using System;

namespace ApiContracts;

public class CommentDto
{
    public int Id { get; set; } = 0;
    public string Body { get; set; } = string.Empty;
    public int PostId { get; set; } = 0;
    public int UserId { get; set; } = 0;

    public PostDto? Post { get; set; }
    public UserDto? User { get; set; }

}
