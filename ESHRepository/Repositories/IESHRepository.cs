using ESHRepository.Interfaces.Repositories;
using ESHRepository.Model;

namespace ESHRepository.Repositories
{
    public interface IESHRepository
    {
        ICRUDRepository<EquipmentType> EquipmentRepository { get; }
        ICRUDRepository<Supplier> SuppliersRepository { get; }
        ISupplyRepository SuppliesRepository { get; }
        IAnalytic AnalyticRepository { get; }
    }
}
