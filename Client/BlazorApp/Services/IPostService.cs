using System;
using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<PostDto> UpdatePostAsync(int id, UpdatePostDto request);
    Task<PostDto?> GetPostAsync(int id);
    Task<List<PostDto>> GetAllPostsAsync(string? title = null, string? createdByName = null, int? createdById = null);
    Task DeleteAsync(int id);
}

