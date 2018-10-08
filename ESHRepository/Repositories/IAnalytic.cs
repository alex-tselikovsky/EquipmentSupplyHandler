using ESHRepository.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESHRepository.Interfaces.Repositories
{
    public interface IAnalytic
    {
        Task<IEnumerable<EquipmentCount>> GetMonthEquipmentCount(int month, int year, string supplierId);
        Task<IEnumerable<SupplierRatio>> GetSupplierRatio(int year);
    }
}
