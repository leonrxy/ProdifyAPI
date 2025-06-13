using System.Threading.Tasks;
using MongoDB.Driver;
using Prodify.Dtos;

namespace Prodify.Infrastructures.Utility
{
    public static class MongoQueryUtility
    {

        public static async Task<PaginatedResponseDto<T>> PaginateAsync<T>(
            this IFindFluent<T, T> find,
            int page_number,
            int page_size)
            where T : class
        {
            // total dokumen matching filter
            var totalCount = await find.CountDocumentsAsync();

            // ambil page yang diinginkan
            var items = await find
                .Skip((page_number - 1) * page_size)
                .Limit(page_size)
                .ToListAsync();

            return new PaginatedResponseDto<T>(
                items,
                (int)totalCount,
                page_number,
                page_size
            );
        }
    }
}
