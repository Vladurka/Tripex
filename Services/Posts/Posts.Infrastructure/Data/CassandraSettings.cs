namespace Posts.Infrastructure.Data;

public class CassandraSettings
{
    public List<string> ScyllaDbContactPoints { get; set; } = new();
    public string ScyllaDbUsername { get; set; } = string.Empty;
    public string ScyllaDbPassword { get; set; } = string.Empty;
    public string ScyllaDbKeyspace { get; set; } = string.Empty;
}
