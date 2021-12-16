using System;

namespace Denok.Lib.Database.Redis
{
    public interface IRedisConfig
    {
        string ClientName { get; set; }
        string Host { get; set; }

        ushort Port { get; set; }

        string Auth { get; set; }

        bool Ssl { get; set; }
    }

    public sealed class RedisConfig : IRedisConfig
    {

        public string ClientName { get; set; }
        
        public string Host { get; set; }

        public ushort Port { get; set; }

        public string Auth { get; set; }

        public bool Ssl { get; set; }
    }
}