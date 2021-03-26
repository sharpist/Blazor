using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Server.Data
{
    public class EFRepository<T> : IRepository<T>
        where T : class, IEquatable<T>, IComparable<T>
    {
        private readonly AppDbContext<T> context;
        public EFRepository(AppDbContext<T> context) =>
            this.context = context;

        public IQueryable<T> Items => context.Entities.AsQueryable();

        public async Task<IEnumerable> TakeAsync() =>
            await context.Entities.AsNoTracking().ToArrayAsync();

        public async Task<T> TakeAsync(int id)
        {
            var dbEntry = await context.Entities.FindAsync(id);
            context.Entry(dbEntry).State = EntityState.Detached;

            return dbEntry;
        }

        public async Task SaveAsync(dynamic entity)
        {
            if (entity.Id == 0) context.Entities.Add(entity);
            else
                context.Entry(entity).State = EntityState.Modified;

            await context.SaveChangesAsync();
        }

        public async Task<T> DeleteAsync(int id)
        {
            var dbEntry = await context.Entities.FindAsync(id);
            if (dbEntry is not null) {
                context.Entities.Remove(dbEntry);

                await context.SaveChangesAsync();
            }
            return dbEntry;
        }
    }
}
