using AyahGraphApi.Domain.Entities;

namespace AyahGraphApi.Domain.Repositories;

public interface IVerseRelationRepository
{
    Task<VerseRelation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<VerseRelation>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        VerseRelation relation,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        VerseRelation relation,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}