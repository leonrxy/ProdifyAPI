using BCrypt.Net;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Prodify.Repositories;
using Microsoft.AspNetCore.Identity;
using Prodify.Helpers;
using Prodify.Models;
using Prodify.Dtos;
using MongoDB.Bson;
using AutoMapper;
using Prodify.Dtos.UserDto;

namespace Prodify.Services
{

    public interface IUserService
    {
        Task<User> RegisterAsync(string email, string password, string role);
        Task<string> AuthenticateAsync(string email, string password);
        Task<PaginatedResponseDto<ListDto>> GetPaginatedAsync(UserPaginatedRequest request);
        Task<DetailDto> GetByIdAsync(string id);
        Task CreateAsync(CreateUserRequestDto request);
        Task UpdateAsync(string id, UpdateUserRequestDto request);
        Task DeleteAsync(string id);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly JwtSettings _jwt;
        private readonly IPasswordHasher _hasher;
        private readonly IRepository<User> _users;
        private readonly IMapper _mapper;

        public UserService(
            IUnitOfWork uow,
            IOptions<JwtSettings> jwtOptions,
            IPasswordHasher hasher,
            IMapper mapper)
        {
            _uow = uow;
            _jwt = jwtOptions.Value;
            _hasher = hasher;
            _users = _uow.Repository<User>();
            _mapper = mapper;

        }

        public async Task<User> RegisterAsync(string email, string password, string role)
        {
            if (await _uow.User.GetByEmailAsync(email) != null)
                throw new Exception("Email already in use");

            var user = new User
            {
                email = email,
                password = _hasher.Hash(password),
                role = role
            };

            await _uow.User.CreateAsync(user);
            return user;
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var user = await _uow.User.GetByEmailAsync(email)
                       ?? throw new Exception("Invalid credentials");

            if (!_hasher.Verify(password, user.password))
                throw new Exception("Invalid credentials");

            // Buat JWT
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.id),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.Role, user.role)
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

        public async Task<PaginatedResponseDto<ListDto>> GetPaginatedAsync(UserPaginatedRequest request)
        {
            var user = await _uow.User.GetPaginatedAsync(request);
            return _mapper.Map<PaginatedResponseDto<ListDto>>(user);

        }

        public async Task<DetailDto> GetByIdAsync(string id)
        {
            var user = await _uow.User.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User dengan id {id} tidak ditemukan");
            var userMap = _mapper.Map<DetailDto>(user);
            return userMap;
        }

        public async Task CreateAsync(CreateUserRequestDto request)
        {
            var existing = await _users.FindAsync(u => u.email == request.email);
            if (await _users.ExistsAsync(u => u.email == request.email))
                throw new InvalidOperationException("Email sudah digunakan oleh user lain");
            if (await _users.ExistsAsync(u => u.username == request.username))
                throw new InvalidOperationException("Username sudah digunakan oleh user lain");

            var user = new User
            {
                id = ObjectId.GenerateNewId().ToString(),
                name = request.name,
                email = request.email,
                username = request.username,
                password = _hasher.Hash(request.password),
                role = request.role,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            await _users.CreateAsync(user);
        }

        public async Task UpdateAsync(string id, UpdateUserRequestDto request)
        {
            var user = await _users.GetByIdAsync(id);
            user.name = request.name;
            user.email = request.email;
            user.username = request.username;
            user.role = request.role;
            user.password = _hasher.Hash(request.password);
            user.updated_at = DateTime.UtcNow;

            await _uow.User.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            if (!await _uow.User.ExistsAsync(id))
                throw new KeyNotFoundException($"User dengan id {id} tidak ditemukan");
            await _uow.User.DeleteAsync(id);
        }
    }
}