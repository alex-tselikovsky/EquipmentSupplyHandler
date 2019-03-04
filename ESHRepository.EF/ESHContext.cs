using ESHRepository.EF.Model;
using ESHRepository.Interfaces.Model;
using ESHRepository.Model;
using Microsoft.EntityFrameworkCore;

namespace ESHRepository.EF
{
    public class ESHContext : DbContext
    {
        public DbQuery<EquipmentCount> EquipmentCount { get; set; }
        public DbQuery<SupplierRatioDB> SupplierRatio { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public ESHContext(DbContextOptions<ESHContext> options):base(options)
        {
            Database.EnsureCreated();
        }
    }
}
