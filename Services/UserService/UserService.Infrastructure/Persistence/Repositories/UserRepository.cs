using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _db;

    public UserRepository(UserDbContext db) => _db = db;

    public Task<User?> GetByLoginAsync(string login, CancellationToken ct)
        => _db.Users.FirstOrDefaultAsync(u => u.Login == login, ct);

    public Task<bool> ExistsAsync(string login, CancellationToken ct)
        => _db.Users.AnyAsync(u => u.Login == login, ct);

    public async Task<User> AddAsync(User user, CancellationToken ct)
    {
        var tmp = await _db.Users.AddAsync(user, ct);
        return tmp.Entity;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await _db.SaveChangesAsync(ct);
    }
}
