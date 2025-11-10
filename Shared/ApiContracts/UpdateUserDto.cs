using System;

namespace ApiContracts;

public class UpdateUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string? Email { get; set; }
    public UpdateUserDto(int id, string userName, string password, string? email)
    {
        Id = id;
        UserName = userName;
        Password = password;
        Email = email;
    }
    public UpdateUserDto()
    {
        Id = 0;
        UserName = string.Empty;
        Password = string.Empty;
        Email = null;
    }
}