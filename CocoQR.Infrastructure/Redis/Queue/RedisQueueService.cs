using CocoQR.Application.Contracts.IQueue;
using CocoQR.Infrastructure.Redis.Cache;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CocoQR.Infrastructure.Redis.Queue
{
    public class RedisQueueService : IQueueService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisQueueService> _logger;

        public RedisQueueService(IConnectionMultiplexer redis, ILogger<RedisQueueService> logger)
        {
            _redis = redis;
            _logger = logger;
        }

        public async Task EnqueueAsync<T>(string queue, T data)
        {
            if (!_redis.IsConnected)
            {
                _logger.LogWarning("Redis not connected, bypass queue");
                return;
            }
            var db = _redis.GetDatabase();

            var json = JsonSerializer.Serialize(data);
            await db.ListRightPushAsync(queue, json);
        }

        public async Task<T?> DequeueAsync<T>(string queue)
        {
            if (!_redis.IsConnected)
            {
                _logger.LogWarning("Redis not connected, bypass queue");
                return default;
            }
            var db = _redis.GetDatabase();

            var value = await db.ListLeftPopAsync(queue);

            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value);
        }
    }
}
