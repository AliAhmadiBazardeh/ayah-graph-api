using AyahGraphApi.Domain.Entities;
using AyahGraphApi.Domain.Enums;

namespace AyahGraphApi.Domain.Tests.Entities;

public class VerseRelationTests
{
    [Fact]
    public void Constructor_Should_CreateRelation_When_InputIsValid()
    {
        // Arrange
        var id = Guid.NewGuid();
        var sourceVerseId = 1;
        var targetVerseId = 2;
        var type = RelationType.Semantic;

        // Act
        var relation = new VerseRelation(
            id,
            sourceVerseId,
            targetVerseId,
            type);

        // Assert
        Assert.Equal(id, relation.Id);
        Assert.Equal(sourceVerseId, relation.SourceVerseId);
        Assert.Equal(targetVerseId, relation.TargetVerseId);
        Assert.Equal(type, relation.Type);
    }

    [Fact]
    public void Constructor_Should_ThrowException_When_IdIsEmpty()
    {
        // Act
        var action = () => new VerseRelation(
            Guid.Empty,
            1,
            2,
            RelationType.Semantic);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Constructor_Should_ThrowException_When_SourceVerseIdIsInvalid()
    {
        // Act
        var action = () => new VerseRelation(
            Guid.NewGuid(),
            0,
            2,
            RelationType.Semantic);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void Constructor_Should_ThrowException_When_TargetVerseIdIsInvalid()
    {
        // Act
        var action = () => new VerseRelation(
            Guid.NewGuid(),
            1,
            0,
            RelationType.Semantic);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void Constructor_Should_ThrowException_When_SourceAndTargetAreSame()
    {
        // Act
        var action = () => new VerseRelation(
            Guid.NewGuid(),
            1,
            1,
            RelationType.Semantic);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Fact]
    public void Constructor_Should_ThrowException_When_RelationTypeIsInvalid()
    {
        // Act
        var action = () => new VerseRelation(
            Guid.NewGuid(),
            1,
            2,
            (RelationType)999);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }
    
    [Fact]
    public void Update_Should_UpdateRelation_When_InputIsValid()
    {
        // Arrange
        var relation = new VerseRelation(
            Guid.NewGuid(),
            1,
            2,
            RelationType.Semantic);

        // Act
        relation.Update(
            3,
            4,
            RelationType.Conceptual);

        // Assert
        Assert.Equal(3, relation.SourceVerseId);
        Assert.Equal(4, relation.TargetVerseId);
        Assert.Equal(RelationType.Conceptual, relation.Type);
    }
    
    [Fact]
    public void Update_Should_ThrowException_When_SourceAndTargetAreSame()
    {
        // Arrange
        var relation = new VerseRelation(
            Guid.NewGuid(),
            1,
            2,
            RelationType.Semantic);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            relation.Update(
                3,
                3,
                RelationType.Semantic));
    }
}