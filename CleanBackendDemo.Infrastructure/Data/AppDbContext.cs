using CleanBackendDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanBackendDemo.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Person> People => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("People");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                  .ValueGeneratedNever();
            entity.Property(e => e.FullName)
                  .HasMaxLength(200)
                  .IsRequired();
        });
    }
}

