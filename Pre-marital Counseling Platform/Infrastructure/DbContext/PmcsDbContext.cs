using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;

namespace SWP391.Infrastructure.DbContext;

public class PmcsDbContext : IdentityDbContext
{
    public new DbSet<User> Users { get; set; }

    public PmcsDbContext(DbContextOptions<PmcsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);
            entity.Property(u => u.Password).HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.FullName).HasMaxLength(100);
            entity.Property(u => u.Role).IsRequired();
        });
    }
}
