using System;

namespace ApiContracts;

public class CommentDto
{
    public int Id { get; set; } = 0;
    public int PostId { get; set; } = 0;
    public int UserId { get; set; } = 0;
}
