using ESHRepository.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESHRepository
{
    public interface ICRUDRepository<T> where T : GUIDEntity
    {
        Task CreateAsync(T entity);
        Task<T> GetAsync(string guid);
        Task RemoveAsync(string guid);
        Task UpdateAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
