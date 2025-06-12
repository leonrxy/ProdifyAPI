using System.Threading.Tasks;
using MongoDB.Driver;
using Prodify.Dtos;

namespace Prodify.Infrastructures.Utility
{
    public static class MongoQueryUtility
    {

        public static async Task<PaginatedResponseDto<T>> PaginateAsync<T>(
            this IFindFluent<T, T> find,
            int pageNumber,
            int pageSize)
        {
            // total dokumen matching filter
            var totalCount = await find.CountDocumentsAsync();

            // ambil page yang diinginkan
            var items = await find
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PaginatedResponseDto<T>(
                items,
                (int)totalCount,
                pageNumber,
                pageSize
            );
        }
    }
}
