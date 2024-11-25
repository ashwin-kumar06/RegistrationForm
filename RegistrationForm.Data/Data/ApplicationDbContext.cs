using RegistrationForm.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace RegistrationForm.Data
{
    public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserDetail> Users { get; set; }
    public DbSet<UserCredential> UserCredentials { get; set; }
    public DbSet<UserAddress> UserAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserDetail>()
            .HasOne(u => u.Credential)
            .WithOne(c => c.User)
            .HasForeignKey<UserCredential>(c => c.UserId);

        modelBuilder.Entity<UserDetail>()
            .HasOne(u => u.Address)
            .WithOne(a => a.User)
            .HasForeignKey<UserAddress>(a => a.UserId);
    }
}
}