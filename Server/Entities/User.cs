using System;

namespace Entities;

public class User(string Username, string Password, string Email)
{
    public int Id { get; set; } = 0;
    public string Username { get; set; } = Username;
    public string Password { get; set; } = Password;
    public string Email { get; set; } = Email;
}
