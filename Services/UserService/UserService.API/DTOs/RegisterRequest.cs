namespace UserService.API.DTOs;

public record RegisterRequest(string Login, string Password, string FullName);
