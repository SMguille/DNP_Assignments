using System;
using System.ComponentModel.DataAnnotations;

namespace ApiContracts;

public class PostDto
{
    public int Id { get; set; } = 0;
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int UserId { get; set; } = 0;

    public UserDto? Author { get; set; }
}
