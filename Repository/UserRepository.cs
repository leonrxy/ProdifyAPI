using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Prodify.Dtos;
using Prodify.Infrastructures.Utility;
using Prodify.Models;

namespace Prodify.Repositories
{
    public interface IUserRepository
    {
        Task<PaginatedResponseDto<User>> GetPaginatedAsync(UserPaginatedRequest request);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task CreateAsync(User user);
        Task<User> GetByIdAsync(string id);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string email);
    }

    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDatabase db)
        {
            _users = db.GetCollection<User>("users");
        }

        public async Task<PaginatedResponseDto<User>> GetPaginatedAsync(UserPaginatedRequest req)
        {
            var page = req.page_number < 1 ? 1 : req.page_number;
            var size = req.page_size < 1 ? 10 : req.page_size;

            var filter = Builders<User>.Filter.Empty;
            if (!string.IsNullOrEmpty(req.search))
            {
                filter = Builders<User>.Filter.Or(
                    Builders<User>.Filter.Regex(u => u.Name, new MongoDB.Bson.BsonRegularExpression(req.search, "i")),
                    Builders<User>.Filter.Regex(u => u.Email, new MongoDB.Bson.BsonRegularExpression(req.search, "i"))
                );
            }

            return await _users
                .Find(filter)
                .PaginateAsync(page, size);
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _users
                .Find(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _users
                .Find(u => u.Username == username)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _users
                .Find(u => u.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task UpdateAsync(User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }
        public async Task DeleteAsync(string id)
        {
            await _users.DeleteOneAsync(u => u.Id == id);
        }
        public async Task<bool> ExistsAsync(string email)
        {
            return await _users
                .Find(u => u.Email == email)
                .AnyAsync();
        }
    }
}
