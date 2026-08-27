using AyahGraphApi.Domain.Enums;

namespace AyahGraphApi.Domain.Entities;

public sealed class VerseRelation
{
    public Guid Id { get; private set; }

    public int SourceVerseId { get; private set; }

    public int TargetVerseId { get; private set; }

    public RelationType Type { get; private set; }

    public VerseRelation(
        Guid id,
        int sourceVerseId,
        int targetVerseId,
        RelationType type)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Relation ID cannot be empty.",
                nameof(id));
        }
        
        if (sourceVerseId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(sourceVerseId),
                "Source verse ID must be greater than zero.");
        }
        
        if (targetVerseId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(targetVerseId),
                "Target verse ID must be greater than zero.");
        }
        
        if (sourceVerseId == targetVerseId)
        {
            throw new ArgumentException(
                "Source and target verses cannot be the same.");
        }
        
        if (!Enum.IsDefined(type))
        {
            throw new ArgumentOutOfRangeException(
                nameof(type),
                "Relation type is invalid.");
        }
        
        Id = id;
        SourceVerseId = sourceVerseId;
        TargetVerseId = targetVerseId;
        Type = type;
        
        
    }
    
    public void Update(
        int sourceVerseId,
        int targetVerseId,
        RelationType type)
    {
        if (sourceVerseId == targetVerseId)
        {
            throw new InvalidOperationException(
                "Source and target verses cannot be the same.");
        }

        SourceVerseId = sourceVerseId;
        TargetVerseId = targetVerseId;
        Type = type;
    }  
}