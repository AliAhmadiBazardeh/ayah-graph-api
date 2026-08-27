using AyahGraphApi.Application.DTOs.VerseRelations;
using AyahGraphApi.Domain.Entities;
using AyahGraphApi.Domain.Repositories;

namespace AyahGraphApi.Application.Services;

public sealed class VerseRelationService : IVerseRelationService
{
    private readonly IVerseRelationRepository _repository;

    public VerseRelationService(IVerseRelationRepository repository)
    {
        _repository = repository;
    }

    public async Task<VerseRelationResponse> CreateAsync(
        CreateVerseRelationRequest request,
        CancellationToken cancellationToken = default)
    {
        var relation = new VerseRelation(
            Guid.NewGuid(),
            request.SourceVerseId,
            request.TargetVerseId,
            request.Type);

        await _repository.AddAsync(
            relation,
            cancellationToken);

        return MapToResponse(relation);
    }

    public async Task<VerseRelationResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var relation = await _repository.GetByIdAsync(
            id,
            cancellationToken);

        return relation is null
            ? null
            : MapToResponse(relation);
    }

    public async Task<IReadOnlyList<VerseRelationResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var relations = await _repository.GetAllAsync(
            cancellationToken);

        return relations
            .Select(MapToResponse)
            .ToList();
    }

    private static VerseRelationResponse MapToResponse(
        VerseRelation relation)
    {
        return new VerseRelationResponse(
            relation.Id,
            relation.SourceVerseId,
            relation.TargetVerseId,
            relation.Type);
    }
}