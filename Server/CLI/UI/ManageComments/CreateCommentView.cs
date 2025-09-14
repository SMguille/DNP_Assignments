using System;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView(ICommentRepository CommentRepository, IPostRepository PostRepository, IUserRepository UserRepository)
{
    public async Task CreateCommentAsync()
    {
        Console.WriteLine("Creating a new comment...");
        Console.Write("Enter post ID: ");

        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid post ID.");
            return;
        }

        var post = await PostRepository.GetSingleAsync(postId);
        if (post == null)
        {
            Console.WriteLine("Post not found.");
            return;
        }

        Console.Write("Enter comment body: ");
        string? body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Comment body cannot be empty.");
            return;
        }

        Console.Write("Enter user ID: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }

        var user = await UserRepository.GetSingleAsync(userId);
        if (user == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        Comment newComment = new Comment(body, postId, userId);

        await CommentRepository.AddAsync(newComment);
        Console.WriteLine("Comment created successfully.");
    }
}
