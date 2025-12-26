using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.OrderManagement.Application.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _refreshSecretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey não configurado");
            _refreshSecretKey = _configuration["JwtSettings:RefreshSecretKey"] ?? _secretKey + "_refresh";
            _issuer = _configuration["JwtSettings:Issuer"] ?? "ECommerce.API";
            _audience = _configuration["JwtSettings:Audience"] ?? "ECommerce.API";
        }

        public string GenerateAccessToken(int userId, string email, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessTokenExpiry = DateTime.UtcNow.AddMinutes(15);
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("token_type", "access"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: accessTokenExpiry,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        public ClaimsPrincipal? ValidateAccessToken(string token)
        {
            return ValidateToken(token, _secretKey, validateLifetime: true);
        }

        public ClaimsPrincipal? ValidateRefreshToken(string token)
        {
            return ValidateToken(token, _refreshSecretKey, validateLifetime: true);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, bool isRefreshToken = false)
        {
            var secretKey = isRefreshToken ? _refreshSecretKey : _secretKey;
            return ValidateToken(token, secretKey, validateLifetime: false);
        }

        private ClaimsPrincipal? ValidateToken(string token, string secretKey, bool validateLifetime)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                
                // Primeiro, vamos decodificar o token para debug
                var jwtTokenForDebug = tokenHandler.ReadJwtToken(token);
                Console.WriteLine($"Token claims: {string.Join(", ", jwtTokenForDebug.Claims.Select(c => $"{c.Type}:{c.Value}"))}");
                Console.WriteLine($"Token expires: {jwtTokenForDebug.ValidTo}");
                Console.WriteLine($"Token not before: {jwtTokenForDebug.ValidFrom}");

                var key = Encoding.UTF8.GetBytes(secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = validateLifetime,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = validateLifetime, // Só exigir expiration se validateLifetime for true
                    RequireSignedTokens = true
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                
                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    var tokenType = jwtToken.Claims.FirstOrDefault(c => c.Type == "token_type")?.Value;
                    Console.WriteLine($"Validated token type: {tokenType}, Expires: {jwtToken.ValidTo}");
                }
                
                return principal;
            }
            catch (SecurityTokenNoExpirationException ex)
            {
                Console.WriteLine($"Token without expiration: {ex.Message}");
                return null;
            }
            catch (SecurityTokenExpiredException ex)
            {
                Console.WriteLine($"Token expired: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation error: {ex.Message}");
                return null;
            }
        }
    }
}