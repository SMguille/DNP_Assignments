using System;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView(IPostRepository PostRepository)
{
    public async Task CreateAsync()
    {
        Console.WriteLine("Creating a new post...");
        Console.Write("Enter title: ");
        string? title = Console.ReadLine();
        Console.Write("Enter body: ");
        string? body = Console.ReadLine();
        Console.Write("Enter user ID: ");
        string? userIdInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body) || !int.TryParse(userIdInput, out int userId))
        {
            Console.WriteLine("Title, body cannot be empty and user ID must be a valid number.");
            return;
        }

        Post newPost = new Post(title, body, userId);
        await PostRepository.AddAsync(newPost);
        Console.WriteLine($"Post '{title}' created with ID {newPost.Id} by User {userId}.");
    }
}
