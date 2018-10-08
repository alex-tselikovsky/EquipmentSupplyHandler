using ESHRepository.Model;
using ESHRepository.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESHRepository.EF
{
    public  class SupplyRepository: EFGenericRepository<Supply>, ISupplyRepository
    {
        public SupplyRepository(ESHContext context) : base(context){}

        protected override IQueryable<Supply> Query => base.Query;//.Include(s=>s.EquipmentType);

        public async Task<IEnumerable<Supply>> GetAsync(DateTime start, DateTime finish)
        {
            return await Query.Where(s => s.DeliveryDate > start && s.DeliveryDate < finish).ToArrayAsync();
        }
        
    }
}
