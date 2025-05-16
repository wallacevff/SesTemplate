using Microsoft.EntityFrameworkCore;
using SesTemplate.Infra.Data.Interfaces;

namespace SesTemplate.Infra.Data.Contexts;

public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IContextEntityMap<>)
                .Assembly,
            t => t.
            GetInterfaces()
            .Any(i => i is { IsGenericType: true } &&
                      i.GetGenericTypeDefinition() == typeof(IContextEntityMap<>)));
        base.OnModelCreating(modelBuilder);
    }
}