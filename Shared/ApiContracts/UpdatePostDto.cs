using System;

namespace ApiContracts;

public class UpdatePostDto(string Title, string Body, int UserId, int SubForumId = 0)
{
    public int Id { get; set; } = 0;
    public string Title { get; set; } = Title;
    public string Body { get; set; } = Body;
    public int UserId { get; set; } = UserId;
    public int SubForumId { get; set; } = SubForumId;

    public UpdatePostDto() : this(string.Empty, string.Empty, 0, 0)
    {
    }

}
