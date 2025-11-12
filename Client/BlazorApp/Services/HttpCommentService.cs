using System;
using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CommentDto> AddComment(CreateCommentDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("comments", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        var comment = JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        await PopulateRelatedData(comment);

        return comment;
    }

    public async Task<CommentDto> UpdateComment(int id, UpdateCommentDto request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"comments/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        var comment = JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        await PopulateRelatedData(comment);

        return comment;
    }

    public async Task<CommentDto?> GetComment(int id)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"comments/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        var comment = JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? null;

        if (comment != null)
            await PopulateRelatedData(comment);

        return comment;
    }

public async Task<IEnumerable<CommentDto>> GetAllComments(int? PostId = null, string? createdByName = null, int? createdById = null)
{
    // Build query string dynamically
    var queryParameters = new List<string>();

    if (PostId.HasValue)
        queryParameters.Add($"postId={PostId.Value}");

    if (!string.IsNullOrEmpty(createdByName))
        queryParameters.Add($"createdByName={Uri.EscapeDataString(createdByName)}");

    if (createdById.HasValue)
        queryParameters.Add($"createdById={createdById.Value}");

    string queryString = queryParameters.Any() ? "?" + string.Join("&", queryParameters) : "";

    HttpResponseMessage httpResponse = await client.GetAsync($"comments{queryString}");
    string response = await httpResponse.Content.ReadAsStringAsync();

    if (!httpResponse.IsSuccessStatusCode)
        throw new Exception(response);

    var comments = JsonSerializer.Deserialize<List<CommentDto>>(response, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    }) ?? new List<CommentDto>();

    // Populate related data for each comment
    foreach (var comment in comments)
    {
        await PopulateRelatedData(comment);
    }

    return comments;
}
    public async Task Delete(int id)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync($"comments/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }

    // ---------------- Helper Methods ----------------

    private async Task PopulateRelatedData(CommentDto comment)
    {
        comment.Post = await GetPostAsync(comment.PostId);
        comment.User = await GetUserAsync(comment.UserId);
    }

    private async Task<PostDto?> GetPostAsync(int postId)
    {
        try
        {
            HttpResponseMessage httpResponse = await client.GetAsync($"posts/{postId}");
            if (!httpResponse.IsSuccessStatusCode)
                return null;

            string json = await httpResponse.Content.ReadAsStringAsync();
            var post = JsonSerializer.Deserialize<PostDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Optionally fetch the author
            if (post != null)
            {
                post.Author = await GetUserAsync(post.UserId);
            }

            return post;
        }
        catch
        {
            return null;
        }
    }

    private async Task<UserDto?> GetUserAsync(int userId)
    {
        try
        {
            HttpResponseMessage httpResponse = await client.GetAsync($"users/{userId}");
            if (!httpResponse.IsSuccessStatusCode)
                return null;

            string json = await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDto>(json, new JsonSerializerOptions
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
