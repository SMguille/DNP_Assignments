using System;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView(IPostRepository PostRepository)
{
    public async Task ListPostsAsync()
    {
        Console.WriteLine("Listing posts...");
        var postsQuery = PostRepository.GetMany();
        var posts = await Task.Run(() => postsQuery.ToList()); // Run synchronously on background thread
        foreach (var post in posts)
        {
            Console.WriteLine($"{post.Id}: {post.Title} by User {post.UserId}");
        }
    }
}
