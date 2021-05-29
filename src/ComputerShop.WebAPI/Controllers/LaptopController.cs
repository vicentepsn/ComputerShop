using ComputerShop.WebAPI.DTO;
using ComputerShop.WebAPI.Services;
using ComputerShop.WebAPI.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ComputerShop.WebAPI.Controllers
{
    [ApiController]
    [Route("v1/Laptop")]
    public class LaptopController : Controller
    {
        private readonly LaptopService _laptopService;

        public LaptopController(LaptopService laptopService)
        {
            _laptopService = laptopService;
        }

        [HttpPut]
        public IActionResult AddLaptop([FromBody] LaptopDTO laptop)
        {
            try
            {
                var newLaptopId = _laptopService.AddLaptop(laptop);
                return Ok(newLaptopId);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.Status, ex.Value);
            }
        }

        [HttpGet]
        public IActionResult GetLaptops()
        {
            try
            {
                var laptops = _laptopService.GetLaptops();
                return Ok(laptops);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.Status, ex.Value);
            }
        }
    }
}
