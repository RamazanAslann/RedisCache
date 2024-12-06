using Microsoft.Extensions.Configuration;
using RedisServices.Abstract;
using StackExchange.Redis;

namespace RedisServices.Concrete
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IDatabaseAsync _db;
        private readonly IConfiguration _config;

        public RedisService(IConnectionMultiplexer redisConnection, IConfiguration config)
        {
            _redisConnection = redisConnection;
            _db = _redisConnection.GetDatabase();
            _config = config;
        }

        public async Task Clear(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public void ClearAll()
        {
            var redisEndpoints = _redisConnection.GetEndPoints(true);
            foreach (var redisEndpoint in redisEndpoints)
            {
                var redisServer = _redisConnection.GetServer(redisEndpoint);
                redisServer.FlushAllDatabases();
            }
        }

        public async Task<string> GetValueAsync(string key)
        {
            var value = await _db.StringGetAsync(key);
            return value;
        }

        public async Task<bool> SetValueAsync(string key, string value)
        {
            var expiryTime = int.Parse(_config.GetSection("RedisSettings:ExpiryTime").Value);

            var timeToLive = TimeSpan.FromHours(expiryTime);

            return await _db.StringSetAsync(key, value, timeToLive);
        }
    }
}
