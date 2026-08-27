using AyahGraphApi.Domain.Enums;

namespace AyahGraphApi.Application.DTOs.VerseRelations;

public sealed record UpdateVerseRelationRequest(
    int SourceVerseId,
    int TargetVerseId,
    RelationType Type
);