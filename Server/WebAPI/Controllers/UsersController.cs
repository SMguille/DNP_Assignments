using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;
using ApiContracts;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class UsersController(IUserRepository userRepo) : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        if (await UserNameExists(request.UserName)) return Conflict("Username is already taken.");

        User user = new(request.UserName, request.Password);
        User created = await userRepo.AddAsync(user);
        UserDto dto = new()
        {
            Id = created.Id,
            UserName = created.UserName,
            Email = created.Email
        };
        return Created($"/users/{dto.Id}", dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {    
        if (id != request.Id)
            return BadRequest("Route ID and body ID do not match.");

        var existingUser = await userRepo.GetSingleAsync(id);
        if (existingUser == null)
            return NotFound($"User with ID {id} not found.");

        User updatedUser = new()
        {
            Id = id,
            UserName = request.UserName,
            Password = request.Password,
            Email = request.Email

        };
        await userRepo.UpdateAsync(updatedUser);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await userRepo.GetSingleAsync(id);
        if (user == null)
            return NotFound();

        return Ok(new UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email});
    }


[HttpGet]
public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync(
    [FromQuery] string? userName)
{
    // Fetch users and materialize immediately
    var users = await userRepo.GetMany().ToListAsync();
    // Filter by userName if provided
    if (!string.IsNullOrWhiteSpace(userName))
    {
        users = users
            .Where(u => u.UserName.Contains(userName, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    // Project to UserDto — only primitive fields
    var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email
        }).ToList();

    return Ok(userDtos);
}

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await userRepo.GetSingleAsync(id);
        if (user == null) return NotFound();

        await userRepo.DeleteAsync(id);
        return NoContent();
    }

    private async Task<bool> UserNameExists(string userName)
    {
        return await userRepo.GetMany().AnyAsync(u => u.UserName == userName);
    }
  
}