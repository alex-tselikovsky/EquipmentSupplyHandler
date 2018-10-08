using ESHRepository.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESHRepository.Repositories
{
    public interface ISupplyRepository: ICRUDRepository<Supply>
    {
        Task<IEnumerable<Supply>> GetAsync(DateTime start, DateTime finish);
    }
}
