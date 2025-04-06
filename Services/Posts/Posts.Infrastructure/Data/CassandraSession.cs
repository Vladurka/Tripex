using Cassandra;

public static class CassandraSession
{
    private static ISession _session;

    public static ISession Connect()
    {
        if (_session != null) return _session;

        var cluster = Cluster.Builder()
            .AddContactPoint("localhost") 
            .WithPort(9043)               
            .Build();

        _session = cluster.Connect("tripex");
        return _session;
    }
}