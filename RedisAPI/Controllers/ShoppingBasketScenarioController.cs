using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisAPI.Models.BaketModels;
using RedisServices.Abstract;

namespace RedisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingBasketScenarioController : ControllerBase
    {
        private readonly IBasketService _basketService;
        public ShoppingBasketScenarioController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("get-my-basket/{userId}")]
        public async Task<IActionResult> GetMyBasket(string userId)
        {
            var myBasket = await _basketService.GetBasketAsync(userId);
            return Ok(myBasket);
             
        }

        [HttpPost("add-item-to-basket")]
        public async Task<IActionResult> AddItemToBasket(string userId,BasketItem item)
        {
            await _basketService.AddItemToBasket(userId,item);
            return Ok();
        }

        
    }
}
