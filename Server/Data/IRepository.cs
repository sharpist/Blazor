using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Server.Data
{
    public interface IRepository<T>
    {
        IQueryable<T> Items { get; }

        Task<IEnumerable> TakeAsync();

        Task<T> TakeAsync(Int32 id);

        Task SaveAsync(dynamic entity);

        Task<T> DeleteAsync(Int32 id);
    }
}
