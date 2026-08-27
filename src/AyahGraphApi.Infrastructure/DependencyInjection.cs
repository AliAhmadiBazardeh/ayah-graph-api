using AyahGraphApi.Infrastructure.Neo4j.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neo4j.Driver;
using AyahGraphApi.Domain.Repositories;
using AyahGraphApi.Infrastructure.Neo4j.Repositories;

namespace AyahGraphApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var neo4jOptions = configuration
                               .GetSection(Neo4jOptions.SectionName)
                               .Get<Neo4jOptions>()
                           ?? throw new InvalidOperationException(
                               "Neo4j configuration is missing.");

        services.AddSingleton(neo4jOptions);

        services.AddSingleton<IDriver>(_ =>
            GraphDatabase.Driver(
                neo4jOptions.Uri,
                AuthTokens.Basic(
                    neo4jOptions.Username,
                    neo4jOptions.Password)));
        
        services.AddScoped<
            IVerseRelationRepository,
            Neo4jVerseRelationRepository>();

        return services;
    }
}