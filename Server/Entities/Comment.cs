using System;

namespace Entities;

public class Comment(string Body, int PostId, int UserId)
{
    public int Id { get; set; } = 0;
    public string Body { get; set; } = Body;
    public int PostId { get; set; } = PostId;
    public int UserId { get; set; } = UserId;
}
