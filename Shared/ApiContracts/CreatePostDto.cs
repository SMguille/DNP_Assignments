using System;

namespace ApiContracts;

public class CreatePostDto
{
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int UserId { get; set; } = 0;
    public int SubForumId { get; set; } = 0;

}