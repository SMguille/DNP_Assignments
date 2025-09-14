using System;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView(IPostRepository PostRepository)
{
    public async Task ViewPostAsync()
    {
        Console.Write("Enter post ID to view: ");
        string? idInput = Console.ReadLine();
        if (int.TryParse(idInput, out int postId))
        {
            try
            {
                Post post = await PostRepository.GetSingleAsync(postId);
                Console.WriteLine($"Post ID: {post.Id}");
                Console.WriteLine($"Title: {post.Title}");
                Console.WriteLine($"Body: {post.Body}");
                Console.WriteLine($"User ID: {post.UserId}");
                Console.WriteLine($"SubForum ID: {post.SubForumId}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Invalid post ID.");
        }
    }
}