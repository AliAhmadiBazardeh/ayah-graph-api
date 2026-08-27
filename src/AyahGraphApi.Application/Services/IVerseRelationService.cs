using AyahGraphApi.Application.DTOs.VerseRelations;

namespace AyahGraphApi.Application.Services;

public interface IVerseRelationService
{
    Task<VerseRelationResponse> CreateAsync(
        CreateVerseRelationRequest request,
        CancellationToken cancellationToken = default);

    Task<VerseRelationResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<VerseRelationResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);
    
    
    Task<VerseRelationResponse> UpdateAsync(
        Guid id,
        UpdateVerseRelationRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}