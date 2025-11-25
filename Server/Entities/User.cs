using System;

using System.Text.Json.Serialization; // You might need this later to prevent cycles

namespace Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string? Email { get; set; }

    // Navigation Property: A user has written many posts
    // JsonIgnore is often used here to stop the "Cycle" error mentioned in step 9.3
    [JsonIgnore] 
    public List<Post> Posts { get; set; }

    // Navigation Property: A user has written many comments
    [JsonIgnore]
    public List<Comment> Comments { get; set; }

    public User() 
    {
        Posts = new List<Post>();
        Comments = new List<Comment>();
    }

    public User(string userName, string password) : this()
    {
        UserName = userName;
        Password = password;
    }
    
    public User(string userName, string password, string email)
    {
        UserName = userName;
        Password = password;
        Email = email;
    }
}