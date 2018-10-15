using System;
using System.Threading.Tasks;
using EquipmentSupplyHandler.Notifications;
using ESHRepository.Model;
using ESHRepository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Notifications.Infractructure;

namespace EquipmentSupplyHandler.Controllers
{
    [Produces("application/json")]
    [Route("api/Delivery")]
    public class DeliveryController : Controller
    {
        ISupplyRepository _repo { get; set; }
        ConcurrentDictNotificator<SupplyOperation> Notificator { get; set; }

        public DeliveryController(IESHRepository eshRepo, ConcurrentDictNotificator<SupplyOperation> notificator)
        {
            _repo = eshRepo.SuppliesRepository;
            Notificator = notificator;
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
            Notificator.Notify(item.Id, new SupplyOperation() { Supply = item, Operation = Operation.Created });

            return CreatedAtRoute("GetDelivery", new { id = item.Id }, item);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Supply item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            var db_obj = await _repo.GetAsync(item.Id);
            if (db_obj == null)
            {
                return NotFound();
            }
            await _repo.UpdateAsync(item);
            Notificator.Notify(item.Id, new SupplyOperation() { Supply = item, Operation = Operation.Updated });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await _repo.GetAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            await _repo.RemoveAsync(id);
            Notificator.Notify(id, new SupplyOperation() { Supply = item, Operation = Operation.Deleted });
            return NoContent();
        }

    }
}