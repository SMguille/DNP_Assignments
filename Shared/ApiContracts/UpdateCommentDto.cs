using System;

namespace ApiContracts;

public class UpdateCommentDto(string Body, int PostId, int UserId)
{
    public int Id { get; set; } = 0;
    public string Body { get; set; } = Body;
    public int PostId { get; set; } = PostId;
    public int UserId { get; set; } = UserId;
    public UpdateCommentDto() : this(string.Empty, 0, 0)
    {
    }
}
