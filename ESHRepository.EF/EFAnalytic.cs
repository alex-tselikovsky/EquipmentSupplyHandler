using ESHRepository.EF.Model;
using ESHRepository.Interfaces.Model;
using ESHRepository.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESHRepository.EF
{
    class EFAnalytic : IAnalytic
    {
        ESHContext _context;
        public EFAnalytic(ESHContext eSHContext)
        {
            _context = eSHContext;
        }
        public async Task<IEnumerable<EquipmentCount>> GetMonthEquipmentCount(int month, int year,string supplierId)
        {
            DateTime start = new DateTime(year, month, 1);
            DateTime end = start.AddMonths(1);
            return await _context.EquipmentCount.AsNoTracking().FromSql(
                $@" Select Count = Sum(s.Count), EquipmentName = et.Name , EquipmentId =s.EquipmentTypeId from Supplies s
                inner join EquipmentTypes as et on et.Id = s.EquipmentTypeId
                where s.DeliveryDate >= {start.ToString("yyyy-MM-dd")} 
                and s.DeliveryDate < {end.ToString("yyyy-MM-dd")} 
                and s.SupplierId = {supplierId}
                group by s.EquipmentTypeId, s.SupplierId, et.Name
                order by Count desc").ToArrayAsync();
        }

        public async Task<IEnumerable<SupplierRatio>> GetSupplierRatio(int year)
        {
            DateTime start = new DateTime(year, 1, 1);
            DateTime end = start.AddYears(1);
            var dbResult =  await _context.SupplierRatio.AsNoTracking().FromSql(
                 $@"Select SuppliesCount = Sum(s.Count), SupplierName = sr.Name, SupplierId = s.SupplierId from Supplies s
                inner join EquipmentTypes as et on et.Id = s.EquipmentTypeId
                inner join Suppliers as sr on sr.Id = s.SupplierId
                where s.DeliveryDate >= {start.ToString("yyyy-MM-dd")} 
                and s.DeliveryDate < {end.ToString("yyyy-MM-dd")} 
                group by s.SupplierId, sr.Name
                order by SuppliesCount desc").ToArrayAsync();
            var count = dbResult.Sum(r => r.SuppliesCount);
            return dbResult.Select(r => new SupplierRatio()
            {
                SupplierId = r.SupplierId,
                SupplierName = r.SupplierName,
                Percentage = (double)r.SuppliesCount / count * 100
            });
        }
    }
}
