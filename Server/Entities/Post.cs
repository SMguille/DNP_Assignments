using System;

namespace Entities;

public class Post(int Id, string Title, string Body, int UserID, int SubForumId)
{
    public int Id { get; set; } = Id;
    public string Title { get; set; } = Title;
    public string Body { get; set; } = Body;
    public int UserID { get; set; } = UserID;
    public int SubForumId { get; set; } = SubForumId;
}
