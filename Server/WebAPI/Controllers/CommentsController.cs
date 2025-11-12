using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using ApiContracts;


namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController(ICommentRepository commentRepository, IUserRepository userRepository) : ControllerBase
{
    private readonly ICommentRepository commentRepo = commentRepository;
    private readonly IUserRepository userRepo = userRepository;

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        Comment comment = new(request.Body, request.PostId, request.UserId);
        Comment created = await commentRepo.AddAsync(comment);
        CommentDto dto = new()
        {
            Id = created.Id,
            PostId = created.PostId,
            UserId = created.UserId
        };
        return Created($"/comments/{dto.Id}", dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CommentDto>> UpdateComment(int id, [FromBody] UpdateCommentDto request)
    {
        if (id != request.Id)
            return BadRequest("Route ID and body ID do not match.");

        var existingComment = await commentRepo.GetSingleAsync(id);
        if (existingComment == null)
            return NotFound($"Comment with ID {id} not found.");

        Comment updatedComment = new()
        {
            Id = request.Id,
            Body = request.Body,
            PostId = request.PostId,
            UserId = request.UserId,
        };
        await commentRepo.UpdateAsync(updatedComment);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(int id)
    {
        var comment = await commentRepo.GetSingleAsync(id);
        if (comment == null)
            return NotFound();

        return Ok(new CommentDto { Id = comment.Id, Body = comment.Body, PostId = comment.PostId, UserId = comment.UserId });
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetAllComments(
        [FromQuery] int? PostId,
        [FromQuery] string? createdByName,
        [FromQuery] int? createdById)
    {
        var comments = commentRepo.GetMany();
        
        //Username filter
        if (!string.IsNullOrWhiteSpace(createdByName))
        {
            var users = userRepo.GetMany();

            var matchingUserIds = users
            .Where(u => u.UserName.Contains(createdByName, StringComparison.OrdinalIgnoreCase))
            .Select(u => u.Id)
            .ToHashSet();

            comments = comments.Where(c => matchingUserIds.Contains(c.UserId));
        }
        //ID filter
        if (createdById.HasValue)
        {
            comments = comments.Where(c => c.UserId == createdById.Value);
        }
        //PostId filter
        if (PostId.HasValue)
        {
            comments = comments.Where(c => PostId == c.PostId);
        }

        var commentDtos = comments.Select(c => new CommentDto
        {
            Id = c.Id,
            Body = c.Body,
            PostId = c.PostId,
            UserId = c.UserId
        }).ToList();

        return Ok(commentDtos);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await commentRepo.GetSingleAsync(id);
        if (comment == null) return NotFound();

        await commentRepo.DeleteAsync(id);
        return NoContent();
    }
}