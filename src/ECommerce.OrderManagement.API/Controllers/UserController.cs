using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace ECommerce.OrderManagement.API.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        [EnableRateLimiting("LoginPolicy")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginResponse = await _userService.LoginAsync(loginDto);

            if (loginResponse == null)
            {
                return Unauthorized(new { Message = "Credenciais inválidas" });
            }

            // Opcionalmente, armazenar o refresh token em um cookie httpOnly
            SetRefreshTokenCookie(loginResponse.RefreshToken, loginResponse.RefreshTokenExpiresAt);

            return Ok(new 
            {
                loginResponse.Token,
                loginResponse.UserId,
                loginResponse.Email,
                loginResponse.Role,
                loginResponse.ExpiresAt,
                // Não retornar refresh token no JSON se estiver em cookie
                RefreshToken = loginResponse.RefreshToken
            });
        }

        [HttpPost("refresh-token")]
        [EnableRateLimiting("LoginPolicy")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO? request = null)
        {
            // Tentar obter refresh token do body ou do cookie
            var refreshToken = request?.RefreshToken ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { Message = "Refresh token é obrigatório" });
            }

            var response = await _userService.RefreshTokenAsync(refreshToken);

            if (response == null)
            {
                return Unauthorized(new { Message = "Refresh token inválido ou expirado" });
            }

            // Atualizar cookie com novo refresh token
            SetRefreshTokenCookie(response.RefreshToken, response.RefreshTokenExpiresAt);

            return Ok(new 
            {
                response.Token,
                response.UserId,
                response.Email,
                response.Role,
                response.ExpiresAt,
                RefreshToken = response.RefreshToken
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] RefreshTokenRequestDTO? request = null)
        {
            var refreshToken = request?.RefreshToken ?? Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                _userService.RevokeRefreshTokenAsync(refreshToken);
            }

            Response.Cookies.Delete("refreshToken");
            
            return Ok(new { Message = "Logout realizado com sucesso" });
        }

        [HttpPost("register")]
        [EnableRateLimiting("PerIpWrite")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.RegisterAsync(registerDto);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != "Admin" && currentUserId != id.ToString())
            {
                return Forbid();
            }

            return Ok(user);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            var createdUser = await _userService.CreateUserAsync(userDTO);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            if (id != userDTO.Id)
            {
                return BadRequest(new { Message = "The URL ID does not match the user ID." });
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != "Admin" && currentUserId != id.ToString())
            {
                return Forbid();
            }

            var updatedUser = await _userService.UpdateUserAsync(userDTO);

            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("validate-token")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Valid = true,
                UserId = currentUserId,
                Email = currentUserEmail,
                Role = currentUserRole,
                AllClaims = User.Claims.Select(c => new { Type = c.Type, Value = c.Value }).ToArray()
            });
        }

        [HttpGet("debug-claims")]
        [Authorize]
        public IActionResult DebugClaims()
        {
            var claims = User.Claims.Select(c => new { Type = c.Type, Value = c.Value }).ToArray();
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            var authType = User.Identity?.AuthenticationType;

            return Ok(new
            {
                IsAuthenticated = isAuthenticated,
                AuthenticationType = authType,
                Claims = claims
            });
        }

        #region Helper Methods

        private void SetRefreshTokenCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires,
                SameSite = SameSiteMode.Strict,
                Secure = Request.IsHttps,
                Path = "/"
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        #endregion
    }
}