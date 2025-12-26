using ECommerce.OrderManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<UserDTO> CreateUserAsync(UserDTO userDTO);
        Task<UserDTO> UpdateUserAsync(UserDTO userDTO);
        Task<bool> DeleteUserAsync(int id);
        Task<LoginResponseDTO?> LoginAsync(LoginDTO loginDto);
        Task<UserDTO> RegisterAsync(RegisterDTO registerDto);
        Task<LoginResponseDTO?> RefreshTokenAsync(string refreshToken);
        bool RevokeRefreshTokenAsync(string refreshToken);
    }
}
