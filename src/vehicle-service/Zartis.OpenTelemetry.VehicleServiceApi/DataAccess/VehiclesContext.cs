using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Zartis.OpenTelemetry.VehicleServiceApi.Domain;

namespace Zartis.OpenTelemetry.VehicleServiceApi.Persistence
{
    public class VehiclesContext : DbContext
    {
        public VehiclesContext(DbContextOptions<VehiclesContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Vehicle> Vehicles { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasKey(v => v.Vin);
            modelBuilder.Entity<Vehicle>()
                         .Property(x => x.ComponentSerialNumbers)
                         .HasConversion(new ValueConverter<List<Guid>, string>(
                                            v => JsonConvert.SerializeObject(v), // Convert to string for persistence
                                            v => JsonConvert.DeserializeObject<List<Guid>>(v))); // Convert to List<Guid> for use

            // Seeding
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle("ABC")
                .WithComponentSerialNumber(new Guid("fc24d48c-9182-4b50-9b7b-a2c4a0e0a2ee"))
                .WithComponentSerialNumber(new Guid("303fefff-d076-424d-933a-c793c2e21fab"))
                .WithComponentSerialNumber(new Guid("cb67fb05-1cee-4d18-bdf6-462a9ee3b06c"))
                .WithComponentSerialNumber(new Guid("f83c19e7-e5a5-47be-a400-5ab098770009")),
                new Vehicle("DEF")
                .WithComponentSerialNumber(new Guid("84d016f6-d822-48a8-b860-b52cd3c874d8"))
                .WithComponentSerialNumber(new Guid("835109f0-6d1d-4282-8298-a0b445eb2d0a"))
                .WithComponentSerialNumber(new Guid("3d033481-4b99-4f06-82e8-eedeb0ef90e3"))
                .WithComponentSerialNumber(new Guid("45349b4b-978b-429a-be40-7e60bba82e62"))
                .WithComponentSerialNumber(new Guid("f5ed4513-8d8b-401c-aab1-d8164cb58b3f"))
                .WithComponentSerialNumber(new Guid("c0b6b88d-6192-4929-91ea-a6a3175ea852"))
            );
        }
    }
}
