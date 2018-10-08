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
    [Route("api/Delivery")]
    public class DeliveryController : Controller
    {
        ISupplyRepository _repo { get; set; }
        public DeliveryController(IESHRepository eshRepo)
        {
            _repo = eshRepo.SuppliesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetDelivery")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _repo.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet("inperiod")]
        public async Task<IActionResult> GetInPeriod(DateTime start, DateTime finish)
        {
            var item = await _repo.GetAsync(start, finish);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Supply item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            await _repo.CreateAsync(item);
            return CreatedAtRoute("GetDelivery", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Supply item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            var db_obj = await _repo.GetAsync(id);
            if (db_obj == null)
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