using System;

namespace Denok.Web.Config 
{
    public class InvalidConfigException : Exception 
    {
        public InvalidConfigException(string message) : base(message){ }
        public InvalidConfigException(string message, Exception inner) : base(message, inner) { }
    }

    public static class AppConfig 
    {

        // http port
        public static int HttpPortWeb;
        public static int HttpPortRedirector;

        // mongodb
        public static string MongoDbConnectionRead;
        public static string MongoDbNameRead;
        public static string MongoDbConnectionWrite;
        public static string MongoDbNameWrite;

        public static string DomainName;
        public static string DomainNotFound;

        public static string QrLogo;

        public static void Init()
        {
            DotNetEnv.Env.Load();

            try 
            {
                InitHttpPortWeb();
                InitHttpPortRedirector();

                InitMongoDbConnectionRead();
                InitMongoDbNameRead();

                InitMongoDbConnectionWrite();
                InitMongoDbNameWrite();

                InitDomainName();
                InitDomainNotFound();

                InitQrLogo();
            } catch(InvalidConfigException ex)
            {
                throw ex;
            }
        }

        private static void InitHttpPortWeb()
        {
            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HTTP_PORT_WEB")))
            {
                throw new InvalidConfigException("HTTP_PORT_WEB cannot be empty");
            }

            HttpPortWeb = int.Parse(Environment.GetEnvironmentVariable("HTTP_PORT_WEB"));
        }

        private static void InitHttpPortRedirector()
        {
            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HTTP_PORT_REDIRECTOR")))
            {
                throw new InvalidConfigException("HTTP_PORT_REDIRECTOR cannot be empty");
            }

            HttpPortRedirector = int.Parse(Environment.GetEnvironmentVariable("HTTP_PORT_REDIRECTOR"));
        }

        private static void InitMongoDbConnectionRead()
        {
            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGO_DB_CONNNECTION_READ")))
            {
                throw new InvalidConfigException("MONGO_DB_CONNNECTION_READ cannot be empty");
            }

            MongoDbConnectionRead = Environment.GetEnvironmentVariable("MONGO_DB_CONNNECTION_READ");
        }

        private static void InitMongoDbNameRead()
        {
            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGO_DB_NAME_READ")))
            {
                throw new InvalidConfigException("MONGO_DB_NAME_READ cannot be empty");
            }

            MongoDbNameRead = Environment.GetEnvironmentVariable("MONGO_DB_NAME_READ");
        }

        private static void InitMongoDbConnectionWrite()
        {
            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGO_DB_CONNNECTION_WRITE")))
            {
                throw new InvalidConfigException("MONGO_DB_CONNNECTION_WRITE cannot be empty");
            }

            MongoDbConnectionWrite = Environment.GetEnvironmentVariable("MONGO_DB_CONNNECTION_WRITE");
        }

        private static void InitMongoDbNameWrite()
        {
            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGO_DB_NAME_WRITE")))
            {
                throw new InvalidConfigException("MONGO_DB_NAME_WRITE cannot be empty");
            }

            MongoDbNameWrite = Environment.GetEnvironmentVariable("MONGO_DB_NAME_WRITE");
        }

        private static void InitDomainName()
        {
            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOMAIN_NAME")))
            {
                throw new InvalidConfigException("DOMAIN_NAME cannot be empty");
            }

            DomainName = Environment.GetEnvironmentVariable("DOMAIN_NAME");
        }

        private static void InitDomainNotFound()
        {
            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOMAIN_NOT_FOUND")))
            {
                throw new InvalidConfigException("DOMAIN_NOT_FOUND cannot be empty");
            }

            DomainNotFound = Environment.GetEnvironmentVariable("DOMAIN_NOT_FOUND");
        }

        private static void InitQrLogo() 
        {
            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("QR_LOGO")))
            {
                throw new InvalidConfigException("QR_LOGO cannot be empty");
            }

            QrLogo = Environment.GetEnvironmentVariable("QR_LOGO");
        }
    }
}