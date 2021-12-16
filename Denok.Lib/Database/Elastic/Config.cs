using System;

namespace Denok.Lib.Database.Elastic
{
    public interface IElasticConfig
    {
        string Host { get; set; }

        string Username { get; set; }

        string Password { get; set; }

        ushort Port { get; set; }
    }

    public sealed class ElasticConfig : IElasticConfig
    {
        public string Host { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public ushort Port { get; set; }
    }
}