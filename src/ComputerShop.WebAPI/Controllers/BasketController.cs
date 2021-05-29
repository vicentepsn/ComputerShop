using ComputerShop.WebAPI.DTO;
using ComputerShop.WebAPI.Services;
using ComputerShop.WebAPI.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ComputerShop.WebAPI.Controllers
{
    [ApiController]
    [Route("v1/Basket")]
    public class BasketController : Controller
    {
        private readonly BasketService _basketService;

        public BasketController(BasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPut("item")]
        public IActionResult AddBasketItem([FromBody] BasketItemDTO basketItem)
        {
            try
            {
                var newBasketItemId = _basketService.AddBasketItem(basketItem);
                return Ok(newBasketItemId);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.Status, ex.Value);
            }
        }

        [HttpGet]
        public IActionResult GetBasketItem()
        {
            try
            {
                var laptops = _basketService.GetBasketItems();
                return Ok(laptops);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.Status, ex.Value);
            }
        }

    }
}
