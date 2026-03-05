using FluentResults;
using MediatR;

namespace UserService.Application.Commands.RegisterUser;

public record RegisterUserCommand(
    string Login,
    string Password,
    string FullName) : IRequest<Result<Guid>>;
