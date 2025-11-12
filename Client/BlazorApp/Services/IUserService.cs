using System;
using ApiContracts;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task UpdateUserAsync(int id, UpdateUserDto request);
    Task<UserDto> GetUSer(int id);
    Task<List<UserDto>> GetAllAsync(string? userName);
    Task<IActionResult> Delete(int id);
}