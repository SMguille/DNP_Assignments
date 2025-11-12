using System;

namespace ApiContracts;

public class UserDto
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }

    public UserDto() { }

    public UserDto(int id, string userName)
    {
        Id = id;
        UserName = userName;
    }
    public UserDto(int id, string userName, string email)
    {
        Id = id;
        UserName = userName;
        Email = email;
    }
}