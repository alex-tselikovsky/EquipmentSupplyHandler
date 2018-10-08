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
    [Route("api/equipment")]
    public class EquipmentController : Controller
    {
        ICRUDRepository<EquipmentType> equipmentRepo { get; set; }
        public EquipmentController(IESHRepository eshRepo)
        {
            equipmentRepo = eshRepo.EquipmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await equipmentRepo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetEquipment")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await equipmentRepo.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EquipmentType item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            await equipmentRepo.CreateAsync(item);
            return CreatedAtRoute("GetEquipment", new { id = item.Id }, item);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EquipmentType item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            var db_obj = await equipmentRepo.GetAsync(item.Id);
            if (db_obj == null)
            {
                return NotFound();
            }
            await equipmentRepo.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await equipmentRepo.RemoveAsync(id);
            return NoContent();
        }

    }
}