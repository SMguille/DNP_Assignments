using System;

namespace Entities;

public class User(int Id, string Username, string Password)
{
    public int Id { get; set; } = Id;
    public string Username { get; set; } = Username;
    public string Password { get; set; } = Password;



}
