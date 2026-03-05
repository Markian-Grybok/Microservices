using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByLoginAsync(string login, CancellationToken ct);
    Task<bool> ExistsAsync(string login, CancellationToken ct);
    Task<User> AddAsync(User user, CancellationToken ct);
    Task<int> SaveChangesAsync(CancellationToken ct);
}
