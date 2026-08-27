using AyahGraphApi.Application.DTOs.VerseRelations;
using AyahGraphApi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AyahGraphApi.Controllers;

[ApiController]
[Route("api/v1/verse-relations")]
public sealed class VerseRelationsController : ControllerBase
{
    private readonly IVerseRelationService _service;

    public VerseRelationsController(
        IVerseRelationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<VerseRelationResponse>> Create(
        CreateVerseRelationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VerseRelationResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VerseRelationResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(
            id,
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<VerseRelationResponse>> Update(
        Guid id,
        UpdateVerseRelationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }
}