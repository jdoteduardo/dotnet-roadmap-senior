using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Interfaces;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;
using ECommerce.OrderManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly JwtService _jwtService;
        private static readonly Dictionary<string, RefreshTokenInfo> _refreshTokens = new();

        public UserService(IRepository<User> userRepository, IMapper mapper, JwtService jwtService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDTO?>(user);
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            var createdUser = await _userRepository.AddAsync(user);
            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task<UserDTO> UpdateUserAsync(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            var updatedUser = await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserDTO>(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<LoginResponseDTO?> LoginAsync(LoginDTO loginDto)
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email.Value == loginDto.Email);

            if (user == null || !user.IsActive || !VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            var accessToken = _jwtService.GenerateAccessToken(user.Id, user.Email.Value, user.Role);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _refreshTokens[refreshToken] = new RefreshTokenInfo
            {
                UserId = user.Id,
                Email = user.Email.Value,
                Role = user.Role,
                ExpiresAt = refreshTokenExpiry,
                CreatedAt = DateTime.UtcNow
            };

            return new LoginResponseDTO
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Email = user.Email.Value,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7)
            };
        }

        public async Task<LoginResponseDTO?> RefreshTokenAsync(string refreshToken)
        {
            if (!_refreshTokens.TryGetValue(refreshToken, out var tokenInfo))
            {
                return null;
            }

            if (tokenInfo.IsExpired)
            {
                _refreshTokens.Remove(refreshToken);
                return null;
            }

            var user = await _userRepository.GetByIdAsync(tokenInfo.UserId);
            if (user == null || !user.IsActive)
            {
                _refreshTokens.Remove(refreshToken);
                return null;
            }

            var newAccessToken = _jwtService.GenerateAccessToken(tokenInfo.UserId, tokenInfo.Email, tokenInfo.Role);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            _refreshTokens.Remove(refreshToken);

            var newRefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            _refreshTokens[newRefreshToken] = new RefreshTokenInfo
            {
                UserId = tokenInfo.UserId,
                Email = tokenInfo.Email,
                Role = tokenInfo.Role,
                ExpiresAt = newRefreshTokenExpiry,
                CreatedAt = DateTime.UtcNow
            };

            return new LoginResponseDTO
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                UserId = tokenInfo.UserId,
                Email = tokenInfo.Email,
                Role = tokenInfo.Role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                RefreshTokenExpiresAt = newRefreshTokenExpiry
            };
        }

        public bool RevokeRefreshTokenAsync(string refreshToken)
        {
            if (_refreshTokens.ContainsKey(refreshToken))
            {
                _refreshTokens.Remove(refreshToken);
                return true;
            }

            return false;
        }

        public async Task<UserDTO> RegisterAsync(RegisterDTO registerDto)
        {
            var users = await _userRepository.GetAllAsync();
            if (users.Any(u => u.Email.Value == registerDto.Email))
            {
                throw new InvalidOperationException("Email já está em uso");
            }

            var user = new User
            {
                Name = registerDto.Name,
                Email = new Email(registerDto.Email),
                PasswordHash = HashPassword(registerDto.Password),
                Role = registerDto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddAsync(user);
            return _mapper.Map<UserDTO>(createdUser);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }

    public class RefreshTokenInfo
    {
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    }
}
