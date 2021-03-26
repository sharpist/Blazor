using Microsoft.EntityFrameworkCore;
using System;

namespace Blazor.Server.Data
{
    public class AppDbContext<T> : DbContext
        where T : class, IEquatable<T>, IComparable<T>
    {
        // имя будущей БД можно указать через
        // вызов конструктора базового класса
        public AppDbContext(DbContextOptions<AppDbContext<T>> options)
            : base(options) // имя БД/строка подключения к БД
        { }

        public DbSet<T> Entities { get; set; }
    }
}
