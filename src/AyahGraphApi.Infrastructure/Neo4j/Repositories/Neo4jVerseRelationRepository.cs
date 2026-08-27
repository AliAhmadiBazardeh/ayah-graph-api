using AyahGraphApi.Infrastructure.Neo4j.Configuration;
using AyahGraphApi.Domain.Entities;
using AyahGraphApi.Domain.Enums;
using AyahGraphApi.Domain.Exceptions;
using AyahGraphApi.Domain.Repositories;
using Neo4j.Driver;

namespace AyahGraphApi.Infrastructure.Neo4j.Repositories;

public sealed class Neo4jVerseRelationRepository : IVerseRelationRepository
{
    private readonly IDriver _driver;
    private readonly string _database;

    public Neo4jVerseRelationRepository(
        IDriver driver,
        Neo4jOptions options)
    {
        _driver = driver;
        _database = options.Database;
    }

    public async Task AddAsync(
        VerseRelation relation,
        CancellationToken cancellationToken = default)
    {
        var relationshipType = GetRelationshipType(relation.Type);

        await using var session = _driver.AsyncSession(
            options => options.WithDatabase(_database));

        var query = $@"
                    MERGE (source:Ayah {{id: $sourceVerseId}})
                    MERGE (target:Ayah {{id: $targetVerseId}})
                    CREATE (source)-[r:{relationshipType} {{id: $relationId}}]->(target)
                    ";

        await session.ExecuteWriteAsync(
            async tx =>
            {
                await tx.RunAsync(
                    query,
                    new
                    {
                        sourceVerseId = relation.SourceVerseId,
                        targetVerseId = relation.TargetVerseId,
                        relationId = relation.Id.ToString()
                    });

                return true;
            });
    }

    public async Task<VerseRelation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await using var session = _driver.AsyncSession(
            options => options.WithDatabase(_database));

        const string query = """
                             MATCH (source:Ayah)-[r]->(target:Ayah)
                             WHERE r.id = $relationId
                             RETURN
                                 source.id AS sourceVerseId,
                                 target.id AS targetVerseId,
                                 r.id AS relationId,
                                 type(r) AS relationType
                             """;

        return await session.ExecuteReadAsync(
            async tx =>
            {
                var cursor = await tx.RunAsync(
                    query,
                    new
                    {
                        relationId = id.ToString()
                    });

                var records = await cursor.ToListAsync();

                var record = records.FirstOrDefault();

                return record is null
                    ? null
                    : MapToVerseRelation(record);
            });
    }

    public async Task<IReadOnlyList<VerseRelation>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        await using var session = _driver.AsyncSession(
            options => options.WithDatabase(_database));

        const string query = """
                             MATCH (source:Ayah)-[r]->(target:Ayah)
                             RETURN
                                 source.id AS sourceVerseId,
                                 target.id AS targetVerseId,
                                 r.id AS relationId,
                                 type(r) AS relationType
                             """;

        return await session.ExecuteReadAsync(
            async tx =>
            {
                var cursor = await tx.RunAsync(query);

                var records = await cursor.ToListAsync();

                return records
                    .Select(MapToVerseRelation)
                    .ToList();
            });
    }

    public async Task UpdateAsync(
        VerseRelation relation,
        CancellationToken cancellationToken = default)
    {
        var relationshipType = GetRelationshipType(relation.Type);

        await using var session = _driver.AsyncSession(
            options => options.WithDatabase(_database));

        var query = $@"
        MATCH (oldSource:Ayah)-[oldRelation:SEMANTIC|CONCEPTUAL]->(oldTarget:Ayah)
        WHERE oldRelation.id = $relationId

        DELETE oldRelation

        MERGE (source:Ayah {{id: $sourceVerseId}})
        MERGE (target:Ayah {{id: $targetVerseId}})

        CREATE (source)-[r:{relationshipType} {{id: $relationId}}]->(target)
        RETURN r.id AS relationId
        ";

        await session.ExecuteWriteAsync(
            async tx =>
            {
                var cursor = await tx.RunAsync(
                    query,
                    new
                    {
                        relationId = relation.Id.ToString(),
                        sourceVerseId = relation.SourceVerseId,
                        targetVerseId = relation.TargetVerseId
                    });

                var records = await cursor.ToListAsync();

                if (records.Count == 0)
                {
                    throw new VerseRelationNotFoundException(
                        relation.Id);
                }

                return true;
            });
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await using var session = _driver.AsyncSession(
            options => options.WithDatabase(_database));

        const string query = """
                             MATCH ()-[r:SEMANTIC|CONCEPTUAL]->()
                             WHERE r.id = $relationId
                             DELETE r
                             RETURN $relationId AS relationId
                             """;

        await session.ExecuteWriteAsync(
            async tx =>
            {
                var cursor = await tx.RunAsync(
                    query,
                    new
                    {
                        relationId = id.ToString()
                    });

                var records = await cursor.ToListAsync();

                if (records.Count == 0)
                {
                    throw new VerseRelationNotFoundException(id);
                }

                return true;
            });
    }
    
    private static string GetRelationshipType(RelationType type)
    {
        return type switch
        {
            RelationType.Semantic => "SEMANTIC",
            RelationType.Conceptual => "CONCEPTUAL",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
    private static RelationType ParseRelationshipType(
        string relationshipType)
    {
        return relationshipType switch
        {
            "SEMANTIC" => RelationType.Semantic,
            "CONCEPTUAL" => RelationType.Conceptual,
            _ => throw new InvalidOperationException(
                $"Unknown relationship type: {relationshipType}")
        };
    }
    
    private static VerseRelation MapToVerseRelation(IRecord record)
    {
        var sourceVerseId =
            record["sourceVerseId"].As<int>();

        var targetVerseId =
            record["targetVerseId"].As<int>();

        var relationId =
            Guid.Parse(record["relationId"].As<string>());

        var relationType = ParseRelationshipType(
            record["relationType"].As<string>());

        return new VerseRelation(
            relationId,
            sourceVerseId,
            targetVerseId,
            relationType);
    }
}