using StackExchange.Redis;
using System.Text.Json;

namespace GrocerySupermarket.Infrastructure.Data
{
    public class CacheService
    {
        private readonly IDatabase _db;

        public CacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        // Store a string with optional expiration
        public async Task SetString(string key, string value, TimeSpan? expiry = null)
        {
            await _db.StringSetAsync(key, value, expiry);
        }

        public async Task<string?> GetString(string key)
        {
            var value = await _db.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        // Generic object caching to simplify usage
        public async Task SetObject<T>(string key, T obj, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(obj);
            await _db.StringSetAsync(key, json, expiry);
        }

        public async Task<T?> GetObject<T>(string key)
        {
            var redisValue = await _db.StringGetAsync(key);

            // Return default if key does not exist
            if (!redisValue.HasValue)
                return default;

            // Convert RedisValue to string safely
            string json = redisValue.ToString()!;

            // Deserialize safely
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
