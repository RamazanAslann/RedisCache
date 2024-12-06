using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedisServices.Abstract;
using RedisServices.Concrete;
using StackExchange.Redis;

namespace RedisServices
{
    public static class Registration
    {
        public static void AddRedis(this IServiceCollection services,IConfiguration configuration) 
        {
            services.AddSingleton<IRedisService, RedisService>();
            services.AddSingleton<IBasketService, BasketService>();
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConnection = configuration.GetSection("RedisSettings:Connection").Value;
                
                return ConnectionMultiplexer.Connect(redisConnection);
            });

        }
    }
}
