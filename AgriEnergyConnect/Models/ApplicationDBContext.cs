using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Farmer> Farmers { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure relationships and constraints
        builder.Entity<Product>()
            .HasOne(p => p.Farmer)
            .WithMany(f => f.Products)
            .HasForeignKey(p => p.FarmerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Farmer>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Farmer>()
            .HasIndex(f => f.Email);

        builder.Entity<Farmer>()
            .HasIndex(f => f.UserId);

        builder.Entity<Farmer>()
            .HasIndex(f => f.FarmName);

        builder.Entity<Product>()
            .HasIndex(p => p.Category);

        builder.Entity<Product>()
            .HasIndex(p => p.ProductionDate);

        builder.Entity<Product>()
            .HasIndex(p => new { p.FarmerId, p.Category });
    }
}