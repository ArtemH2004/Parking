using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Parking.Models;

namespace Parking.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Contract> Contracts { get; set; } = null!;
        public DbSet<Guard> Guards { get; set; } = null!;
        public DbSet<ParkingLot> ParkingLots { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<Driver> Drivers { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });

            modelBuilder.Entity<Contract>()
                .Property(c => c.Amount)
                .HasColumnType("decimal(18,2)"); 

            modelBuilder.Entity<Driver>()
                .Property(d => d.Salary)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Guard>()
                .Property(g => g.Salary)
                .HasColumnType("decimal(18,2)");


            // Client - Vehicle: One to Many
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Vehicles)
                .WithOne(v => v.Client)
                .HasForeignKey(v => v.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            //// Client - Contract: One to Many
            //modelBuilder.Entity<Client>()
            //    .HasMany(c => c.Contracts)
            //    .WithOne(con => con.Client)
            //    .HasForeignKey(con => con.ClientId)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contract>()
               .HasOne(c => c.Client)
               .WithMany(g => g.Contracts)
               .HasForeignKey(c => c.ClientId)
               .OnDelete(DeleteBehavior.Restrict);

            ////// Driver - Contract: One to Many
            //modelBuilder.Entity<Driver>()
            //    .HasMany(d => d.Contracts)
            //    .WithOne(con => con.Driver)
            //    .HasForeignKey(con => con.DriverId);

            ////// Guard - Contract: One to Many
            //modelBuilder.Entity<Guard>()
            //    .HasMany(d => d.Contracts)
            //    .WithOne(con => con.Guard)
            //    .HasForeignKey(con => con.GuardId);

            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Guard)
                .WithMany(g => g.Contracts)
                .HasForeignKey(c => c.GuardId)
                .OnDelete(DeleteBehavior.Restrict); // Или .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Driver)
                .WithMany(d => d.Contracts)
                .HasForeignKey(c => c.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            //// ParkingLot - Driver: One to Many
            modelBuilder.Entity<ParkingLot>()
                .HasMany(pl => pl.Drivers)
                .WithOne(d => d.ParkingLot)
                .HasForeignKey(d => d.ParkingLotId)
                .OnDelete(DeleteBehavior.Restrict);

            //// ParkingLot - Guard: One to Many
            modelBuilder.Entity<ParkingLot>()
                .HasMany(pl => pl.Guards)
                .WithOne(g => g.ParkingLot)
                .HasForeignKey(g => g.ParkingLotId)
                .OnDelete(DeleteBehavior.Restrict);

            //// ParkingLot - Contract: One to Many
            modelBuilder.Entity<ParkingLot>()
                .HasMany(pl => pl.Contracts)
                .WithOne(con => con.ParkingLot)
                .HasForeignKey(con => con.ParkingLotId)
                .OnDelete(DeleteBehavior.Restrict);

            //// ParkingLot - ParkingSpace: One to Many
            //modelBuilder.Entity<ParkingLot>()
            //    .HasMany(pl => pl.ParkingSpaces)
            //    .WithOne(ps => ps.ParkingLot)
            //    .HasForeignKey(ps => ps.ParkingLotId);

            //// ParkingSpace - Contract: One to One
            //modelBuilder.Entity<ParkingSpace>()
            //    .HasOne(ps => ps.Contract)
            //    .WithOne(con => con.ParkingSpace)
            //    .HasForeignKey<Contract>(con => con.ParkingSpaceId);

            //// Vehicle - Contract: One to One
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Contract)
                .WithOne(con => con.Vehicle)
                .HasForeignKey<Contract>(con => con.VehicleId);

            //// ParkingType - ParkingLot: One to One
            //modelBuilder.Entity<ParkingType>()
            //    .HasOne(pt => pt.ParkingLot)
            //    .WithOne(pl => pl.ParkingType)
            //    .HasForeignKey<ParkingLot>(pl => pl.ParkingTypeId);

        }
    }
}
