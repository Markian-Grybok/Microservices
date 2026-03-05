using FluentResults;
using MediatR;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.Commands.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        try
        {
            if (await _userRepository.ExistsAsync(request.Login, ct))
            {
                return Result.Fail(new Error($"User with login '{request.Login}' already exists.")
                    .WithMetadata("ErrorCode", "USER_ALREADY_EXISTS"));
            }

            var hash = _passwordHasher.Hash(request.Password);
            var user = User.Create(request.Login, hash, request.FullName);

            await _userRepository.AddAsync(user, ct);
            var savedRows = await _userRepository.SaveChangesAsync(ct);

            if (savedRows == 0)
            {
                return Result.Fail(new Error("User was not saved.")
                    .WithMetadata("ErrorCode", "USER_CREATION_FAILED"));
            }

            return Result.Ok(user.Id);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Database error during registration.")
                .WithMetadata("ErrorCode", "DB_ERROR")
                .WithMetadata("Details", ex.InnerException?.Message ?? ex.Message));
        }
    }
}
