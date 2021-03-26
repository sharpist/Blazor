using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Server.Extensions
{
    public static class HttpContextExtension
    {
        public static async Task InsertPaginationParameterInResponse<T>(this HttpContext httpContext,
            IQueryable<T> queryable, int recordsPerPage)
        {
            double count = await queryable.CountAsync();
            double pagesQuantity = Math.Ceiling(count / recordsPerPage);
            httpContext.Response.Headers.Add("pagesQuantity", pagesQuantity.ToString());
        }
    }
}
