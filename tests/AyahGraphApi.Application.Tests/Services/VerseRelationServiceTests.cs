using AyahGraphApi.Application.DTOs.VerseRelations;
using AyahGraphApi.Application.Services;
using AyahGraphApi.Application.Tests.Fakes;
using AyahGraphApi.Domain.Enums;

namespace AyahGraphApi.Application.Tests.Services;

public class VerseRelationServiceTests
{
    [Fact]
    public async Task CreateAsync_Should_CreateRelation_When_RequestIsValid()
    {
        // Arrange
        var repository = new FakeVerseRelationRepository();
        var service = new VerseRelationService(repository);

        var request = new CreateVerseRelationRequest(
            SourceVerseId: 1,
            TargetVerseId: 2,
            Type: RelationType.Semantic);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(1, result.SourceVerseId);
        Assert.Equal(2, result.TargetVerseId);
        Assert.Equal(RelationType.Semantic, result.Type);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnRelation_When_RelationExists()
    {
        // Arrange
        var repository = new FakeVerseRelationRepository();
        var service = new VerseRelationService(repository);

        var createdRelation = await service.CreateAsync(
            new CreateVerseRelationRequest(
                1,
                2,
                RelationType.Semantic));

        // Act
        var result = await service.GetByIdAsync(createdRelation.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdRelation.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_When_RelationDoesNotExist()
    {
        // Arrange
        var repository = new FakeVerseRelationRepository();
        var service = new VerseRelationService(repository);

        // Act
        var result = await service.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnAllRelations()
    {
        // Arrange
        var repository = new FakeVerseRelationRepository();
        var service = new VerseRelationService(repository);

        await service.CreateAsync(
            new CreateVerseRelationRequest(
                1,
                2,
                RelationType.Semantic));

        await service.CreateAsync(
            new CreateVerseRelationRequest(
                3,
                4,
                RelationType.Conceptual));

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task UpdateAsync_Should_UpdateRelation()
    {
        // Arrange
        var repository = new FakeVerseRelationRepository();
        var service = new VerseRelationService(repository);

        var createdRelation = await service.CreateAsync(
            new CreateVerseRelationRequest(
                1,
                2,
                RelationType.Semantic));

        var request = new UpdateVerseRelationRequest(
            3,
            4,
            RelationType.Conceptual);

        // Act
        var result = await service.UpdateAsync(
            createdRelation.Id,
            request);

        // Assert
        Assert.Equal(
            createdRelation.Id,
            result.Id);

        Assert.Equal(
            3,
            result.SourceVerseId);

        Assert.Equal(
            4,
            result.TargetVerseId);

        Assert.Equal(
            RelationType.Conceptual,
            result.Type);
    }
    
    [Fact]
    public async Task DeleteAsync_Should_DeleteRelation()
    {
        // Arrange
        var repository = new FakeVerseRelationRepository();
        var service = new VerseRelationService(repository);

        var createdRelation = await service.CreateAsync(
            new CreateVerseRelationRequest(
                1,
                2,
                RelationType.Semantic));

        // Act
        await service.DeleteAsync(createdRelation.Id);

        var result = await service.GetByIdAsync(
            createdRelation.Id);

        // Assert
        Assert.Null(result);
    }
}