using AyahGraphApi.Domain.Enums;

namespace AyahGraphApi.Application.DTOs.VerseRelations;

public sealed record CreateVerseRelationRequest(
    int SourceVerseId,
    int TargetVerseId,
    RelationType Type
);