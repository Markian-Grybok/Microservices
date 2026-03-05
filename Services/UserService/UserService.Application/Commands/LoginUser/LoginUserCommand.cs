using FluentResults;
using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Commands.LoginUser;

public record LoginUserCommand(
    string Login,
    string Password) : IRequest<Result<TokenDto>>;
