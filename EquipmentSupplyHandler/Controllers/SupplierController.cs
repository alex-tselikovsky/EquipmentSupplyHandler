using System.Threading.Tasks;
using ESHRepository;
using ESHRepository.Model;
using ESHRepository.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentSupplyHandler.Controllers
{
    [Produces("application/json")]
    [Route("api/supplier")]
    public class SupplierController : Controller
    {
        ICRUDRepository<Supplier> _repo { get; set; }
        public SupplierController(IESHRepository eshRepo)
        {
            _repo = eshRepo.SuppliersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetSupplier")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _repo.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Supplier item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            await _repo.CreateAsync(item);
            return CreatedAtRoute("GetSupplier", new { id = item.Id }, item);
        }

        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] Supplier item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            var contactObj = await _repo.GetAsync(item.Id);
            if (contactObj == null)
            {
                return NotFound();
            }
            await _repo.UpdateAsync(item);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repo.RemoveAsync(id);
            return NoContent();
        }
    }
}