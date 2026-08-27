using AyahGraphApi.Domain.Entities;
using AyahGraphApi.Domain.Repositories;

namespace AyahGraphApi.Application.Tests.Fakes;

public sealed class FakeVerseRelationRepository : IVerseRelationRepository
{
    private readonly Dictionary<Guid, VerseRelation> _relations = [];

    public Task<VerseRelation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        _relations.TryGetValue(id, out var relation);

        return Task.FromResult(relation);
    }

    public Task<IReadOnlyList<VerseRelation>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<VerseRelation> relations = _relations.Values.ToList();

        return Task.FromResult(relations);
    }

    public Task AddAsync(
        VerseRelation relation,
        CancellationToken cancellationToken = default)
    {
        _relations.Add(relation.Id, relation);

        return Task.CompletedTask;
    }

    public Task UpdateAsync(
        VerseRelation relation,
        CancellationToken cancellationToken = default)
    {
        _relations[relation.Id] = relation;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        _relations.Remove(id);

        return Task.CompletedTask;
    }
}