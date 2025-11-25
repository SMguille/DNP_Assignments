using System;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    // Add a new post
    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("posts", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        var post = JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        post.Author = await GetAuthorAsync(post.UserId);
        return post;
    }

    // Update an existing post
    public async Task<PostDto> UpdatePostAsync(int id, UpdatePostDto request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"posts/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        var post = JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        post.Author = await GetAuthorAsync(post.UserId);
        return post;
    }

    // Get a single post with author
    public async Task<PostDto?> GetPostAsync(int id)
    {
        // We add query parameters to tell the server to include the data we need
        return await client.GetFromJsonAsync<PostDto>($"posts/{id}?includeAuthor=true&includeComments=true");
    }
    // Get all posts with authors
    public async Task<List<PostDto>> GetAllPostsAsync(string? title = null, string? createdByName = null, int? createdById = null)
    {
        HttpResponseMessage httpResponse = await client.GetAsync("posts");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        var posts = JsonSerializer.Deserialize<List<PostDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<PostDto>();

        foreach (var post in posts)
        {
            post.Author = await GetAuthorAsync(post.UserId);
        }

        return posts;
    }

    // Delete a post
    public async Task DeleteAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync($"posts/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }

    // Helper to fetch author info
    private async Task<UserDto?> GetAuthorAsync(int userId)
    {
        try
        {
            HttpResponseMessage userResponse = await client.GetAsync($"users/{userId}");
            if (!userResponse.IsSuccessStatusCode)
                return null;

            string userJson = await userResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDto>(userJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return null;
        }
    }
}
