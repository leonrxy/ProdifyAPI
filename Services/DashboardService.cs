using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Prodify.Repositories;
using Prodify.Helpers;
using Prodify.Models;
using AutoMapper;
using Prodify.Dtos.DashboardDto;

namespace Prodify.Services
{

    public interface IDashboardService
    {
        Task<ListDto> GetTotalUsersProductsAsync();
    }

    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _uow;
        private readonly JwtSettings _jwt;
        private readonly IPasswordHasher _hasher;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _users;
        private readonly IRepository<Product> _products;
        public DashboardService(
            IUnitOfWork uow,
            IOptions<JwtSettings> jwtOptions,
            IPasswordHasher hasher,
            IMapper mapper)
        {
            _uow = uow;
            _jwt = jwtOptions.Value;
            _hasher = hasher;
            _mapper = mapper;
            _users = _uow.Repository<User>();
            _products = _uow.Repository<Product>();
        }

        public async Task<ListDto> GetTotalUsersProductsAsync()
        {
            var totalUsers = await _users.GetCountAsync();
            var totalProducts = await _products.GetCountAsync();
            return new ListDto
            {
                total_users = totalUsers,
                total_products = totalProducts
            };
        }
    }
}