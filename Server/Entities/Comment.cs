using System;

namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    
    // Foreign Keys
    public int PostId { get; set; }
    public int UserId { get; set; }

    // Navigation Property: A comment belongs to a specific Post
    public Post Post { get; set; }
    
    // Navigation Property: A comment is written by a specific User
    public User User { get; set; }

    public Comment() {}

    public Comment(string body, int postId, int userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }
}