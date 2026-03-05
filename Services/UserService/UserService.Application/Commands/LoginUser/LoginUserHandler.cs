using FluentResults;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Application.Commands.LoginUser;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<TokenDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUserHandler(
        IUserRepository userRepository,
        ITokenService tokenService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<TokenDto>> Handle(LoginUserCommand request, CancellationToken ct)
    {
        try
        {
            var user = await _userRepository.GetByLoginAsync(request.Login, ct);

            if (user is null)
            {
                return Result.Fail(new Error("Invalid login or password.")
                    .WithMetadata("ErrorCode", "INVALID_CREDENTIALS"));
            }

            if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                return Result.Fail(new Error("Invalid login or password.")
                    .WithMetadata("ErrorCode", "INVALID_CREDENTIALS"));
            }

            var token = _tokenService.GenerateToken(user);
            return Result.Ok(new TokenDto(token));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Database error during login.")
                .WithMetadata("ErrorCode", "DB_ERROR")
                .WithMetadata("Details", ex.InnerException?.Message ?? ex.Message));
        }
    }
}