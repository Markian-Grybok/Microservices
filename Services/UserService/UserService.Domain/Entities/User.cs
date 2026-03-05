namespace UserService.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Login { get; private set; }
    public string PasswordHash { get; private set; }
    public string FullName { get; private set; }

    private User() { }

    public static User Create(string login, string passwordHash, string fullName)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Login = login,
            PasswordHash = passwordHash,
            FullName = fullName
        };
    }
}
