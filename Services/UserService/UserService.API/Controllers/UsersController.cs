using Microsoft.AspNetCore.Mvc;
using SharedKernel.Controllers;
using UserService.API.DTOs;
using UserService.Application.Commands.LoginUser;
using UserService.Application.Commands.RegisterUser;
using UserService.Application.DTOs;

namespace UserService.API.Controllers;

public class UsersController : BaseApiController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        return HandleResult(await Mediator.Send(
            new RegisterUserCommand(request.Login, request.Password, request.FullName), ct));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        return HandleResult(await Mediator.Send(
            new LoginUserCommand(request.Login, request.Password), ct));
    }
}
