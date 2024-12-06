using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisAPI.Models;
using RedisServices.Abstract;

namespace RedisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IRedisService redisService;

        public RedisController(IRedisService redisService)
        {
            this.redisService = redisService;
        }

        [HttpPost("set-cache")]
        public async Task<IActionResult> SetCache([FromBody] RedisRequest request)
        {
            await redisService.SetValueAsync(request.Key,request.Value);

            return Ok();
        }

        [HttpPost("get-from-cache/{key}")]
        public async Task<IActionResult> Get(string key)
        {
            return Ok(await redisService.GetValueAsync(key));
        }

        [HttpPost("delete-from-cache/{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            await redisService.Clear(key);
            return Ok();
        }


    }
}
