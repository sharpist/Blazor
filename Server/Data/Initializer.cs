using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Blazor.Server.Data
{
    public class Initializer<T> where T : class, IEquatable<T>, IComparable<T>
    {
        public static void EnsurePopulated(IServiceProvider serviceProvider, params T[] data)
        {
            using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            var context = serviceScope.ServiceProvider.GetService<AppDbContext<T>>();

            context.Database.Migrate();

            if (context.Entities.Any()) return;

            else if (data is not null && data.Any()) {
                context.Entities.AddRange(data);

                context.SaveChanges();
            }
        }
    }
}
