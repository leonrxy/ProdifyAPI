using Microsoft.Extensions.Options;
using Prodify.Repositories;
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
        
        public async Task<PaginatedResponseDto<ListDto>> GetPaginatedAsync(UserPaginatedRequest request)
        {
            var user = await _uow.User.GetPaginatedAsync(request);
            return _mapper.Map<PaginatedResponseDto<ListDto>>(user);

        }

        public async Task<DetailDto> GetByIdAsync(string id)
        {
            var user = await _uow.User.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with id {id} not found");
            var userMap = _mapper.Map<DetailDto>(user);
            return userMap;
        }

        public async Task CreateAsync(CreateUserRequestDto request)
        {
            var existing = await _users.FindAsync(u => u.Email == request.Email);
            if (await _users.ExistsAsync(u => u.Email == request.Email))
                throw new InvalidOperationException("Email sudah digunakan oleh user lain");
            if (await _users.ExistsAsync(u => u.Username == request.Username))
                throw new InvalidOperationException("Username sudah digunakan oleh user lain");

            var user = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = request.Name,
                Email = request.Email,
                Username = request.Username,
                Password = _hasher.Hash(request.Password),
                Role = request.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _users.CreateAsync(user);
        }

        public async Task UpdateAsync(string id, UpdateUserRequestDto request)
        {
            var user = await _users.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User dengan id {id} tidak ditemukan");
            if (await _users.ExistsAsync(u => u.Email == request.Email && u.Id != id))
                throw new InvalidOperationException("Email sudah digunakan oleh user lain");
            if (await _users.ExistsAsync(u => u.Username == request.Username && u.Id != id))
                throw new InvalidOperationException("Username sudah digunakan oleh user lain");
            _mapper.Map(request, user);
            user.UpdatedAt = DateTime.UtcNow;
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