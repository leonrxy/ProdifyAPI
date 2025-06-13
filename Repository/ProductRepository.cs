using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Prodify.Dtos;
using Prodify.Infrastructures.Utility;
using Prodify.Models;

namespace Prodify.Repositories
{
    public interface IProductRepository
    {
        Task<PaginatedResponseDto<Product>> GetPaginatedAsync(ProductPaginatedRequest request);
        Task CreateAsync(Product product);
        Task<Product> GetByIdAsync(string id);
        Task UpdateAsync(Product product);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(IMongoDatabase db)
        {
            _products = db.GetCollection<Product>("products");
        }

        public async Task<PaginatedResponseDto<Product>> GetPaginatedAsync(ProductPaginatedRequest req)
        {
            var page = req.page_number < 1 ? 1 : req.page_number;
            var size = req.page_size < 1 ? 10 : req.page_size;

            var filter = Builders<Product>.Filter.Empty;
            if (!string.IsNullOrEmpty(req.search))
            {
                filter = Builders<Product>.Filter.Or(
                    Builders<Product>.Filter.Regex(u => u.Name, new MongoDB.Bson.BsonRegularExpression(req.search, "i")),
                    Builders<Product>.Filter.Regex(u => u.Description, new MongoDB.Bson.BsonRegularExpression(req.search, "i"))
                );
            }

            return await _products
                .Find(filter)
                .PaginateAsync(page, size);
        }
        public async Task<Product> GetByIdAsync(string id)
        {
            return await _products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task CreateAsync(Product product)
        {
            await _products.InsertOneAsync(product);
        }
        public async Task UpdateAsync(Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
        }
        public async Task DeleteAsync(string id)
        {
            await _products.DeleteOneAsync(p => p.Id == id);
        }
        public async Task<bool> ExistsAsync(string id)
        {
            return await _products
                .Find(p => p.Id == id)
                .AnyAsync();
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            return await _products
                .Find(p => p.Name == name)
                .FirstOrDefaultAsync();
        }
        // public async Task<Product> GetByCategoryAsync(string category)
        // {
        //     return await _products
        //         .Find(p => p.Category == category)
        //         .FirstOrDefaultAsync();
        // }

    }
}