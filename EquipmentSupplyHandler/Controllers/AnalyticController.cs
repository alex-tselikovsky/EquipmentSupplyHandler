using System.Threading.Tasks;
using ESHRepository.Interfaces.Repositories;
using ESHRepository.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentSupplyHandler.Controllers
{
    [Produces("application/json")]
    [Route("api/analytic")]
    public class AnalyticController : Controller
    {
        IAnalytic _repo { get; set; }
        public AnalyticController(IESHRepository eshRepo)
        {
            _repo = eshRepo.AnalyticRepository;
        }

        [HttpGet("MonthEquipmentStatic")]
        public async Task<IActionResult> MonthEquipmentStatic(string supplierId, int month, int year)
        {
            var result = await _repo.GetMonthEquipmentCount(month, year, supplierId);
            return Ok(result);
        }

        [HttpGet("YearSupplierRatio")]
        public async Task<IActionResult> YearSupplierRatio(int year)
        {
            var result = await _repo.GetSupplierRatio(year);
            return Ok(result);
        }

    }
}