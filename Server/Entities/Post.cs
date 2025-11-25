using System;

namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    
    // Foreign Key
    public int UserId { get; set; }
    
    // Navigation Property: A post belongs to one User
    public User User { get; set; } 

    // Navigation Property: A post has many comments
    public List<Comment> Comments { get; set; } 

    public int SubForumId { get; set; }

    // EFC needs this empty constructor
    public Post() 
    {
        // Initialize lists to avoid null errors when creating new objects manually
        Comments = new List<Comment>(); 
    }

    public Post(string title, string body, int userId, int subForumId = 0)
    {
        Title = title;
        Body = body;
        UserId = userId;
        SubForumId = subForumId;
        Comments = new List<Comment>();
    }
}