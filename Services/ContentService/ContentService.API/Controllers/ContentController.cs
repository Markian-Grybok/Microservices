using ContentService.API.DTOs;
using ContentService.Application.Commands.CreateContent;
using ContentService.Application.DTOs;
using ContentService.Application.Queries.GetContentById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Controllers;
using System.Security.Claims;

namespace ContentService.API.Controllers;

public class ContentController : BaseApiController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateContentRequest request, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized(new { message = "Invalid token." });
        }

        return HandleResult(await Mediator.Send(new CreateContentCommand(
            request.Name,
            Guid.Parse(userId),
            request.Payload), ct));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContentDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        return HandleResult(await Mediator.Send(
            new GetContentByIdQuery(id), ct));
    }
}