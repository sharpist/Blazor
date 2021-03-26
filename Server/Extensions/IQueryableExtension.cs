using Blazor.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Server.Extensions
{
    public static class IQueryableExtension
    {
        public static async Task<IEnumerable<T>> Paginate<T>(this IQueryable<T> queryable,
            PaginationDTO pagination) =>
            await queryable
            .Skip((pagination.Page - 1) * pagination.QuantityPerPage)
            .Take(pagination.QuantityPerPage)
            .ToListAsync();
    }
}
