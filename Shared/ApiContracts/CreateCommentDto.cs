using System;

namespace ApiContracts;

public class CreateCommentDto
{
    public string Body { get; set; } = string.Empty;
    public int PostId { get; set; } = 0;
    public int UserId { get; set; } = 0;

}
