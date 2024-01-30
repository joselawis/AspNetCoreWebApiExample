using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitiesManager.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.DataBaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        public ApplicationDbContext() { }

        public virtual DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<City>()
                .HasData(
                    new City()
                    {
                        CityId = Guid.Parse("{ec01d452-7a30-4a23-be5c-516e9ded5783}"),
                        CityName = "London"
                    }
                );
            modelBuilder
                .Entity<City>()
                .HasData(
                    new City()
                    {
                        CityId = Guid.Parse("{f2471756-a8b7-454a-9b10-3011fa4eec26}"),
                        CityName = "Alicante"
                    }
                );
            modelBuilder
                .Entity<City>()
                .HasData(
                    new City()
                    {
                        CityId = Guid.Parse("{5a70b504-01ab-44eb-8ee4-d7e8dc29f6ed}"),
                        CityName = "Berlin"
                    }
                );
        }
    }
}
