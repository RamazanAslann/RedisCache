using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using RedisAPI.Models.BaketModels;
using RedisServices.Abstract;
using StackExchange.Redis;

namespace RedisServices.Concrete
{
    public class BasketService:IBasketService
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IDatabaseAsync _redisDb;
        private readonly IConfiguration _config;
        //private readonly IDistributedCache cache;

        public BasketService(IConnectionMultiplexer redisConnection, IConfiguration config)
        {
            _redisConnection = redisConnection;
            _redisDb = _redisConnection.GetDatabase();
            _config = config;
        }
        public async Task<ShoppingBasket> GetBasketAsync(string userId)
        {
            var basketJson = await _redisDb.StringGetAsync(userId);
            return string.IsNullOrEmpty(basketJson)
                ? new ShoppingBasket { UserId = userId }
                : JsonSerializer.Deserialize<ShoppingBasket>(basketJson);
        }

       

        public  async Task AddItemToBasket(string userId,BasketItem newItem)
        {
            
            var basketJson = await _redisDb.StringGetAsync(userId);
            ShoppingBasket basket;

            
            if (!string.IsNullOrEmpty(basketJson))
            {
                basket = JsonSerializer.Deserialize<ShoppingBasket>(basketJson);
            }
            else
            {
                basket = new ShoppingBasket { UserId = userId };
            }

           
            var existingItem = basket.Items.FirstOrDefault(x => x.ProductId == newItem.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += newItem.Quantity; 
            }
            else
            {
                basket.Items.Add(newItem); 
            }

            
            var updatedBasketJson = JsonSerializer.Serialize(basket);
            await _redisDb.StringSetAsync(userId, updatedBasketJson, TimeSpan.FromHours(int.Parse(_config.GetSection("RedisSettings:ShoppingBasketExpiryTime").Value)));
        }

        public async Task DeleteBasketAsync(string userId)
        {
            await _redisDb.KeyDeleteAsync(userId);
        }
    }
}
