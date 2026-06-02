using System.Text.Json;
using Application.Interfaces;
using StackExchange.Redis;
namespace Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var value = await db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(value!);
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var json = JsonSerializer.Serialize(value);
            if (expirationTime.HasValue)
            {
                await db.StringSetAsync(key, json, expirationTime.Value);
            }
            else
            {
                await db.StringSetAsync(key, json);
            }
        }
        public async Task RemoveAsync(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.KeyDeleteAsync(key);
        }
    }
}