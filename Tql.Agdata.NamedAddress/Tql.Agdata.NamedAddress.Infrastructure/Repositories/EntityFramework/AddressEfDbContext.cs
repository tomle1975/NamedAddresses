using Microsoft.EntityFrameworkCore;
using Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework.Tables;

namespace Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework;

/// <summary>
/// Represents the Address Database.
/// </summary>
public class AddressEfDbContext : DbContext
{
    public DbSet<Address>? Addresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Addresses");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureAddressTable(modelBuilder);
    }

    private static void ConfigureAddressTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>().HasKey(a => a.Id);
        modelBuilder.Entity<Address>().Property(a => a.Id).ValueGeneratedOnAdd();

        modelBuilder.Entity<Address>().HasAlternateKey(a => a.Name);
        modelBuilder.Entity<Address>().Property(a => a.Name).HasMaxLength(50);

        modelBuilder.Entity<Address>().Property(a => a.StreetLine1).IsRequired();
        modelBuilder.Entity<Address>().Property(a => a.StreetLine1).HasMaxLength(150);

        modelBuilder.Entity<Address>().Property(a => a.StreetLine2).HasMaxLength(150);

        modelBuilder.Entity<Address>().Property(a => a.City).IsRequired();
        modelBuilder.Entity<Address>().Property(a => a.City).HasMaxLength(25);

        modelBuilder.Entity<Address>().Property(a => a.State).IsRequired();
        modelBuilder.Entity<Address>().Property(a => a.State).HasMaxLength(15);

        modelBuilder.Entity<Address>().Property(a => a.ZipCode).IsRequired();
        modelBuilder.Entity<Address>().Property(a => a.ZipCode).HasMaxLength(15);

        modelBuilder.Entity<Address>().Property(a => a.Country).IsRequired();
        modelBuilder.Entity<Address>().Property(a => a.Country).HasMaxLength(15);
    }
}