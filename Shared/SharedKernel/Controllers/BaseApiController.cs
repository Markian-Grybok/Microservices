using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??=
        HttpContext.RequestServices.GetService<IMediator>()!;

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Value is null
                ? NotFound(new { error = "Resource not found." })
                : Ok(result.Value);
        }

        var error = result.Errors.First();
        var errorCode = error.Metadata.GetValueOrDefault("ErrorCode")?.ToString();

        return errorCode switch
        {
            "VALIDATION_ERROR" => BadRequest(new
            {
                title = "Validation error.",
                status = 400,
                errors = result.Errors.Select(e => new
                {
                    field = e.Metadata.GetValueOrDefault("Field")?.ToString(),
                    message = e.Message
                })
            }),
            "INVALID_CREDENTIALS" => StatusCode(401, new { detail = error.Message }),
            "USER_ALREADY_EXISTS" => Conflict(new { detail = error.Message }),
            "NOT_FOUND" => NotFound(new { detail = error.Message }),
            "UNAUTHORIZED" => StatusCode(403, new { detail = error.Message }),
            "DB_ERROR" => StatusCode(500, new { detail = error.Message }),
            _ => BadRequest(new { detail = error.Message })
        };
    }
}