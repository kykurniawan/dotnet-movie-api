using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Infrastructure.Database;

public class MovieApiDbContext(DbContextOptions<MovieApiDbContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<MovieTag> MovieTags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        GenerateUuid<Movie>(modelBuilder, "Id");
        SoftDelete<Movie>(modelBuilder);

        GenerateUuid<Tag>(modelBuilder, "Id");
        SoftDelete<Tag>(modelBuilder);

        GenerateUuid<MovieTag>(modelBuilder, "Id");
        SoftDelete<MovieTag>(modelBuilder);
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is Base && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Modified)
            {
                ((Base)entityEntry.Entity).UpdatedAt = DateTime.Now;
            }

            if (entityEntry.State == EntityState.Added)
            {
                ((Base)entityEntry.Entity).CreatedAt = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }

    private static void GenerateUuid<T>(ModelBuilder modelBuilder, string column) where T : Base
    {
        modelBuilder.Entity<T>()
            .HasIndex(CreateExpression<T>(column));

        modelBuilder.Entity<T>()
            .Property(column)
            .HasDefaultValueSql("NEWID()");
    }

    private static Expression<Func<T, object?>> CreateExpression<T>(string uuid) where T : Base
    {
        var type = typeof(T);
        var property = type.GetProperty(uuid);
        var parameter = Expression.Parameter(type);
        Expression access;
        if (property != null) {
            access = Expression.Property(parameter, property);
        } else {
            throw new ArgumentException($"Property '{uuid}' not found on type '{type}'.");
        }
        var convert = Expression.Convert(access, typeof(object));
        var function = Expression.Lambda<Func<T, object?>>(convert, parameter);
    
        return function;
    }

    private static void SoftDelete<T>(ModelBuilder modelBuilder) where T : Base
    {
        modelBuilder.Entity<T>()
            .HasQueryFilter(e => EF.Property<DateTime?>(e, "DeletedAt") == null);
    }
}