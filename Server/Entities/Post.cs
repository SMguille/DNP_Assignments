using System;

namespace Entities;

public class Post(int Id, string Title, string Body, int UserId, int SubForumId)
{
    public int Id { get; set; } = Id;
    public string Title { get; set; } = Title;
    public string Body { get; set; } = Body;
    public int UserId { get; set; } = UserId;
    public int SubForumId { get; set; } = SubForumId;
}
