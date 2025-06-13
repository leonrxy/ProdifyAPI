using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Prodify.Repositories;
using Prodify.Helpers;
using Prodify.Models;

namespace Prodify.Services
{

    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly JwtSettings _jwt;
        private readonly IPasswordHasher _hasher;

        public AuthService(
            IUnitOfWork uow,
            IOptions<JwtSettings> jwtOptions,
            IPasswordHasher hasher)
        {
            _uow = uow;
            _jwt = jwtOptions.Value;
            _hasher = hasher;

        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var user = await _uow.User.GetByEmailAsync(email)
                       ?? throw new Exception("Invalid credentials");

            if (!_hasher.Verify(password, user.Password))
                throw new Exception("Invalid credentials");

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}