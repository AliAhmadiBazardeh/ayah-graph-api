namespace AyahGraphApi.Infrastructure.Neo4j.Configuration;

public sealed class Neo4jOptions
{
    public const string SectionName = "Neo4j";

    public string Uri { get; init; } = string.Empty;

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string Database { get; init; } = "neo4j";
}