using ESHRepository.Interfaces.Repositories;
using ESHRepository.Model;
using ESHRepository.Repositories;

namespace ESHRepository.EF
{
    public class ESHRepository : IESHRepository
    {
        ESHContext _context;
        public ESHRepository(ESHContext context)
        {
            _context = context;
        }
        public ICRUDRepository<EquipmentType> EquipmentRepository => new EFGenericRepository<EquipmentType>(_context);

        public ICRUDRepository<Supplier> SuppliersRepository => new EFGenericRepository<Supplier>(_context);

        public ISupplyRepository SuppliesRepository => new SupplyRepository(_context);

        public IAnalytic AnalyticRepository => new EFAnalytic(_context);
    }
}
