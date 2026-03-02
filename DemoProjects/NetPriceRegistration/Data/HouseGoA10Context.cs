using Microsoft.EntityFrameworkCore;
using NetPriceRegistration.Models;

namespace NetPriceRegistration.Data
{
    public class HouseGoA10Context : DbContext
    {
        public HouseGoA10Context(DbContextOptions<HouseGoA10Context> options) : base(options)
        {
        }

        public DbSet<NetPriceMain> NetPriceMains { get; set; }
        public DbSet<NetPriceCoordinate> NetPriceCoordinates { get; set; }
        public DbSet<NetPriceBuild> NetPriceBuilds { get; set; }
        public DbSet<NetPriceLand> NetPriceLands { get; set; }
        public DbSet<NetPricePark> NetPriceParks { get; set; }
    }
}
