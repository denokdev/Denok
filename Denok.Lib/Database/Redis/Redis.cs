using System;
using Microsoft.Extensions.Logging;
using RedisClient = StackExchange.Redis;

namespace Denok.Lib.Database.Redis
{
    public interface IRedisConnection
    {
        void Connect();

        RedisClient.IDatabase GetDatabase();

        bool IsConnected();

    }

    public sealed class RedisConnection : IDisposable, IRedisConnection
    {
        private readonly ILogger<RedisConnection> _logger;

        private RedisClient.ConnectionMultiplexer _connectionMultiplexer;

        private readonly RedisClient.ConfigurationOptions _configurationOptions;

        public RedisConnection(ILogger<RedisConnection> logger, IRedisConfig config) {
            _logger = logger;

            _configurationOptions = new RedisClient.ConfigurationOptions() {
                EndPoints = {
                    { config.Host, config.Port }
                },
                ClientName = config.ClientName,
                Ssl = config.Ssl,
                Password = config.Auth,
                ConnectTimeout = 5000
            };
           
        }

        public void Connect()
        {
            _logger.LogInformation("connecting redis connection");
            _connectionMultiplexer = RedisClient.ConnectionMultiplexer.Connect(_configurationOptions);
            
        }

        public RedisClient.IDatabase GetDatabase()
        {
            if (!IsConnected())
            {
                this.Connect();
            }

            return _connectionMultiplexer.GetDatabase();
        }

        public bool IsConnected()
        {
            if (_connectionMultiplexer == null)
                return false;

            if (_connectionMultiplexer != null)
            {
                if (!_connectionMultiplexer.IsConnected)
                {
                    return false;
                }
            }

            return true;
 
        }

        public void Dispose()
        {
            _logger.LogInformation("disposing redis connection");
            if (!IsConnected())
            {
                _logger.LogInformation("closing redis connection succeed");
                _connectionMultiplexer.Close();
            }
        }
    }
}