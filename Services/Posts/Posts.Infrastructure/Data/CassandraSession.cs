using Cassandra;
using Microsoft.Extensions.Options;

namespace Posts.Infrastructure.Data
{
    public static class CassandraSession
    {
        private static ISession _session;

        public static ISession Connect(IOptions<CassandraSettings> options)
        {
            if (_session != null) return _session;
            var settings = options.Value;

            var cluster = Cluster.Builder()
                .AddContactPoints(settings.ScyllaDbContactPoints.ToArray())
                .WithCredentials(settings.ScyllaDbUsername, settings.ScyllaDbPassword) 
                .Build();

            _session = cluster.Connect(settings.ScyllaDbKeyspace); 
            return _session;
        }
    }
}