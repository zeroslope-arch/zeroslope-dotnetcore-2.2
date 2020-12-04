using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroSlope.Composition
{
	public class ContainerOptions
	{
		public ContainerOptions()
		{
			Database = new DatabaseSettings();
			Token = new TokenSettings();
			Caching = new CachingSettings();
			CORS = new CorsSettings();
            Consul = new ConsulSettings();
		}

		public DatabaseSettings Database { get; set; }

		public TokenSettings Token { get; set; }

		public CachingSettings Caching { get; set; }

        public CorsSettings CORS { get; set; }

        public ConsulSettings Consul { get; set; }


        public class DatabaseSettings
		{
			public string SqlConnectionString { get; set; }
		}

		public class TokenSettings
		{
			public string Issuer { get; set; }
			public string Audience { get; set; }
            public string Secret { get; set; }
            public int ExpirationInMinutes { get; set; }
        }

		public class CachingSettings
		{
			public string RedisHost { get; set; }
			public int RedisPort { get; set; }
			public int RedisDatabaseId { get; set; }
		}

		public class CorsSettings
		{
			public string PolicyName { get; set; }
			public string[] Origins { get; set; }
		}

        public class ConsulSettings
        {
            public string RegisterAddress { get; set; }
            public string ServiceName { get; set; }
            public string ServiceId { get; set; }
            public List<string> Dependencies { get; set; }
        }
    }
}
