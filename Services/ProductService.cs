using Microsoft.Extensions.Options;
using Prodify.Repositories;
using Prodify.Helpers;
using Prodify.Models;
using Prodify.Dtos;
using AutoMapper;
using Prodify.Dtos.ProductDto;

namespace Prodify.Services
{

    public interface IProductService
    {
        Task<PaginatedResponseDto<ListDto>> GetPaginatedAsync(ProductPaginatedRequest request);
        Task<DetailDto> GetByIdAsync(string id);
        Task CreateAsync(CreateProductRequestDto request, string? adminId = null);
        Task UpdateAsync(string id, UpdateProductRequestDto request);
        Task DeleteAsync(string id);
    }

    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly JwtSettings _jwt;
        private readonly IPasswordHasher _hasher;
        private readonly IRepository<Product> _products;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorage;

        public ProductService(
            IUnitOfWork uow,
            IOptions<JwtSettings> jwtOptions,
            IPasswordHasher hasher,
            IMapper mapper,
            IFileStorageService fileStorage)
        {
            _uow = uow;
            _jwt = jwtOptions.Value;
            _hasher = hasher;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _products = _uow.Repository<Product>();

        }

        public async Task<PaginatedResponseDto<ListDto>> GetPaginatedAsync(ProductPaginatedRequest request)
        {
            var product = await _uow.Product.GetPaginatedAsync(request);
            return _mapper.Map<PaginatedResponseDto<ListDto>>(product);
        }

        public async Task<DetailDto> GetByIdAsync(string id)
        {
            var product = await _uow.Product.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with id {id} not found");
            return _mapper.Map<DetailDto>(product);
        }

        public async Task CreateAsync(CreateProductRequestDto request, string? adminId = null)
        {
            var admin = await _uow.User.GetByIdAsync(adminId);
            AdminInfo adminInfo = null;
            if (admin == null)
            { throw new ArgumentException($"Admin with id '{adminId}' not found"); }
            else
            {
                adminInfo = new AdminInfo
                {
                    Id = admin.Id,
                    Email = admin.Email,
                    Username = admin.Username
                };
            }
            string? savedPath = null;
            if (request.Photo != null)
                savedPath = await _fileStorage.SaveFileAsync(request.Photo, "products");
            var product = _mapper.Map<Product>(request);
            product.PhotoUrl = savedPath ?? null;
            product.Admin = adminInfo;
            await _products.CreateAsync(product);
        }

        public async Task UpdateAsync(string id, UpdateProductRequestDto request)
        {
            var product = await _uow.Product.GetByIdAsync(id);
            if (product == null)
                throw new ArgumentException($"Product with id '{id}' not found");
            var updatedProduct = _mapper.Map(request, product);

            if (request.Photo != null)
            {
                updatedProduct.PhotoUrl = await _fileStorage
                    .ReplaceFileAsync(product.PhotoUrl, request.Photo, "products");
            }
            updatedProduct.UpdatedAt = DateTime.UtcNow;
            await _products.UpdateAsync(updatedProduct);
        }


        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Product ID cannot be null or empty", nameof(id));
            var product = await GetByIdAsync(id);
            await _products.DeleteAsync(id);
        }
    }
}

