using AyahGraphApi.Domain.Enums;

namespace AyahGraphApi.Application.DTOs.VerseRelations;

public sealed record VerseRelationResponse(
    Guid Id,
    int SourceVerseId,
    int TargetVerseId,
    RelationType Type
);