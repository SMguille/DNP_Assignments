using System;

namespace Entities;

public class Post(string Title, string Body, int UserId, int SubForumId = 0)
{
    public int Id { get; set; } = 0;
    public string Title { get; set; } = Title;
    public string Body { get; set; } = Body;
    public int UserId { get; set; } = UserId;
    public int SubForumId { get; set; } = SubForumId;
}
