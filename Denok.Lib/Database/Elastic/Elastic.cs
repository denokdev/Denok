using System;
using Nest;
using Microsoft.Extensions.Logging;

namespace Denok.Lib.Database.Elastic
{
    public interface IElastic
    {
        void Connect();

        ElasticClient GetClient();

        bool IsConnected();
    }

    public sealed class Elastic : IDisposable, IElastic
    {
        private readonly ILogger<Elastic> _logger;

        private ElasticClient _client;

        private ConnectionSettings _connectionSettings;

        public Elastic(ILogger<Elastic> logger, IElasticConfig config)
        {
            _logger = logger;
            
            var node = new Uri(String.Format($"{config.Host}:{config.Port}"));
            _connectionSettings = new ConnectionSettings(node);
            if (!string.IsNullOrEmpty(config.Username))
            {
                _connectionSettings.BasicAuthentication(config.Username, config.Password);
            }
        }

        public void Connect()
        {
            _client = new ElasticClient(_connectionSettings);
        }

        public ElasticClient GetClient()
        {
            return _client;
        }

        public bool IsConnected()
        {
            if (_client == null)
                return false;
            
            if (_client != null)
            {
                var pingResponse = _client.Ping();
                if (!pingResponse.IsValid)
                    return false;
            }

            return true;
        }

        public void Dispose()
        {
            _client = null;
        }
    }
}