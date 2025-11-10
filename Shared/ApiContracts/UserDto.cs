using System;

namespace ApiContracts;

public class UserDto
{
    public int Id { get; set; }
    public string? UserName { get; set; }

    public UserDto() { }

    public UserDto(int id, string userName)
    {
        Id = id;
        UserName = userName;
    }
}