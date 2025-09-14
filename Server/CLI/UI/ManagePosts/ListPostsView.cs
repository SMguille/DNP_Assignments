using System;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView(IPostRepository PostRepository)
{
    public async Task ListPostsAsync()
    {
        Console.WriteLine("Listing posts...");
        var postsQuery = PostRepository.GetManyAsync();
        var posts = postsQuery.ToList();
        foreach (var post in posts)
        {
            Console.WriteLine($"{post.Id}: {post.Title} by User {post.UserId}");
        }
    }
}
