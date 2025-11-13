using System;
using System.Linq;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Mvc;
using ApiContracts;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")] //Route is localhost:XXXX/auth/
public class AuthController(IUserRepository userRepository) : ControllerBase
{
    private readonly IUserRepository _userRepo = userRepository;

    [HttpPost("login")] // endpoint -> auth/login
    public ActionResult<UserDto> LogIn(LoginRequest loginRequest)
    {
        var user = _userRepo.GetMany().FirstOrDefault(u => u.UserName == loginRequest.UserName);
        if (user is null) return Unauthorized();
        if (user.Password != loginRequest.Password) return Unauthorized();
        return new UserDto(user.Id, user.UserName);
    }
}
