using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESHRepository;
using ESHRepository.Model;
using ESHRepository.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentSupplyHandler.Controllers
{
    [Produces("application/json")]
    [Route("api/Supplier")]
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Supplier item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            var contactObj = await _repo.GetAsync(id);
            if (contactObj == null)
            {
                return NotFound();
            }
            await _repo.UpdateAsync(item);
            return NoContent();
        }
    }
}