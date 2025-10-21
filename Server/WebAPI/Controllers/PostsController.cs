using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using ApiContracts;
using System.Data;


namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController] 
public class PostsController(IPostRepository postRepository, IUserRepository userRepository) : ControllerBase
{
    private readonly IPostRepository postRepo = postRepository;
    private readonly IUserRepository userRepo = userRepository;

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        Post post = new(request.Title, request.Body, request.UserId, request.SubForumId);
        Post created = await postRepo.AddAsync(post);
        PostDto dto = new()
        {
            Id = created.Id,
            Title = created.Title,
            UserId = created.UserId
        };
        return Created($"/posts/{dto.Id}", dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PostDto>> UpdatePost(int id, [FromBody] UpdatePostDto request)
    {
        if (id != request.Id)
            return BadRequest("Route ID and body ID do not match.");

        var existingPost = await postRepo.GetSingleAsync(id);
        if (existingPost == null)
            return NotFound($"Post with ID {id} not found.");

        Post updatedPost = new()
        {
            Id = id,
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId,
            SubForumId = request.SubForumId

        };
        await postRepo.UpdateAsync(updatedPost);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(int id)
    {
        var post = await postRepo.GetSingleAsync(id);
        if (post == null)
            return NotFound();

        return Ok(new PostDto { Id = post.Id, Title = post.Title, UserId = post.UserId});
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetAllPosts(
        [FromQuery] string? title,
        [FromQuery] string? createdByName,
        [FromQuery] int? createdById)
    {
        var posts = postRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }
        if (!string.IsNullOrWhiteSpace(createdByName))
        {
            var users = userRepo.GetMany();
            
            var matchingUserIds = users
            .Where(u => u.UserName.Contains(createdByName, StringComparison.OrdinalIgnoreCase))
            .Select(u => u.Id)
            .ToHashSet();

            posts = posts.Where(p => matchingUserIds.Contains(p.UserId));
        }
        if (createdById.HasValue)
        {
            posts = posts.Where(p => p.UserId == createdById.Value);
        }

        var postDtos = posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            UserId = p.UserId
        }).ToList();

        return Ok(postDtos);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await postRepo.GetSingleAsync(id);
        if (post == null) return NotFound();

        await postRepo.DeleteAsync(id);
        return NoContent();
    }
}