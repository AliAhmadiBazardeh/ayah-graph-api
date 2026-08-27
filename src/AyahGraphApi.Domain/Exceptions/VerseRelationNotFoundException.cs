namespace AyahGraphApi.Domain.Exceptions;

public sealed class VerseRelationNotFoundException : Exception
{
    public VerseRelationNotFoundException(Guid relationId)
        : base($"Verse relation with id '{relationId}' was not found.")
    {
        RelationId = relationId;
    }

    public Guid RelationId { get; }
}