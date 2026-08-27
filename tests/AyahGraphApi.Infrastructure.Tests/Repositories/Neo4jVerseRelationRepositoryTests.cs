using AyahGraphApi.Domain.Entities;
using AyahGraphApi.Domain.Enums;
using AyahGraphApi.Domain.Exceptions;
using AyahGraphApi.Infrastructure.Neo4j.Configuration;
using AyahGraphApi.Infrastructure.Neo4j.Repositories;
using AyahGraphApi.Infrastructure.Tests.Configuration;
using Neo4j.Driver;

namespace AyahGraphApi.Infrastructure.Tests.Repositories;

public sealed class Neo4jVerseRelationRepositoryTests
{
    private readonly string _uri;
    private readonly string _username;
    private readonly string _password;
    private readonly string _database;

    public Neo4jVerseRelationRepositoryTests()
    {
        var configuration = TestConfiguration.Build();

        _uri = configuration["Neo4j:Uri"]
               ?? throw new InvalidOperationException(
                   "Neo4j URI is not configured.");

        _username = configuration["Neo4j:Username"]
                    ?? throw new InvalidOperationException(
                        "Neo4j username is not configured.");

        _password = configuration["Neo4j:Password"]
                    ?? throw new InvalidOperationException(
                        "Neo4j password is not configured.");

        _database = configuration["Neo4j:Database"] ?? "neo4j";
    }

    private IDriver CreateDriver()
    {
        return GraphDatabase.Driver(
            _uri,
            AuthTokens.Basic(_username, _password));
    }

    private Neo4jVerseRelationRepository CreateRepository(
        IDriver driver)
    {
        var options = new Neo4jOptions
        {
            Database = _database
        };

        return new Neo4jVerseRelationRepository(
            driver,
            options);
    }

    [Fact]
    public async Task AddAsync_Should_Create_Relation()
    {
        // Arrange
        await using var driver = CreateDriver();

        var repository = CreateRepository(driver);

        var relation = new VerseRelation(
            Guid.NewGuid(),
            1001,
            1002,
            RelationType.Semantic);

        // Act
        await repository.AddAsync(relation);

        var result = await repository.GetByIdAsync(relation.Id);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(
            relation.Id,
            result.Id);

        Assert.Equal(
            relation.SourceVerseId,
            result.SourceVerseId);

        Assert.Equal(
            relation.TargetVerseId,
            result.TargetVerseId);

        Assert.Equal(
            relation.Type,
            result.Type);
    }
    
    [Fact]
    public async Task GetAllAsync_Should_Return_All_Relations()
    {
        // Arrange
        await using var driver = CreateDriver();

        var repository = CreateRepository(driver);

        var firstRelation = new VerseRelation(
            Guid.NewGuid(),
            2001,
            2002,
            RelationType.Semantic);

        var secondRelation = new VerseRelation(
            Guid.NewGuid(),
            2003,
            2004,
            RelationType.Conceptual);

        await repository.AddAsync(firstRelation);
        await repository.AddAsync(secondRelation);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Contains(
            result,
            relation => relation.Id == firstRelation.Id);

        Assert.Contains(
            result,
            relation => relation.Id == secondRelation.Id);
    }
    
    [Fact]
    public async Task UpdateAsync_Should_Update_Relation()
    {
        // Arrange
        await using var driver = CreateDriver();

        var repository = CreateRepository(driver);

        var relation = new VerseRelation(
            Guid.NewGuid(),
            3001,
            3002,
            RelationType.Semantic);

        await repository.AddAsync(relation);

        var updatedRelation = new VerseRelation(
            relation.Id,
            3003,
            3004,
            RelationType.Conceptual);

        // Act
        await repository.UpdateAsync(updatedRelation);

        var result = await repository.GetByIdAsync(
            updatedRelation.Id);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(
            updatedRelation.Id,
            result.Id);

        Assert.Equal(
            updatedRelation.SourceVerseId,
            result.SourceVerseId);

        Assert.Equal(
            updatedRelation.TargetVerseId,
            result.TargetVerseId);

        Assert.Equal(
            updatedRelation.Type,
            result.Type);
    }
    
    [Fact]
    public async Task DeleteAsync_Should_Delete_Relation()
    {
        // Arrange
        await using var driver = CreateDriver();

        var repository = CreateRepository(driver);

        var relation = new VerseRelation(
            Guid.NewGuid(),
            4001,
            4002,
            RelationType.Semantic);

        await repository.AddAsync(relation);

        // Act
        await repository.DeleteAsync(relation.Id);

        var result = await repository.GetByIdAsync(relation.Id);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Relation_Does_Not_Exist()
    {
        // Arrange
        await using var driver = CreateDriver();

        var repository = CreateRepository(driver);

        var relation = new VerseRelation(
            Guid.NewGuid(),
            5001,
            5002,
            RelationType.Semantic);

        // Act & Assert
        await Assert.ThrowsAsync<VerseRelationNotFoundException>(
            () => repository.UpdateAsync(relation));
    }
    
    [Fact]
    public async Task DeleteAsync_Should_Throw_When_Relation_Does_Not_Exist()
    {
        // Arrange
        await using var driver = CreateDriver();

        var repository = CreateRepository(driver);

        var relationId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<VerseRelationNotFoundException>(
            () => repository.DeleteAsync(relationId));
    }
}