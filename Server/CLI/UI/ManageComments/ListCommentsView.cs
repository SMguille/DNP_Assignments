using System;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView(ICommentRepository commentRepository)
{
    public async Task ListCommentsAsync()
    {
        Console.WriteLine("Listing comments...");
        var commentsQuery = commentRepository.GetManyAsync();
        var comments = commentsQuery.ToList(); // Synchronous
        foreach (var comment in comments)
        {
            Console.WriteLine($"{comment.Id}: {comment.Body} on Post {comment.PostId} by User {comment.UserId}");
        }
    }
}
