using System;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView(ICommentRepository commentRepository)
{
    public async Task ListCommentsAsync()
    {
        Console.WriteLine("Listing comments...");
        var commentsQuery = commentRepository.GetMany();
        var comments = await Task.Run(() => commentsQuery.ToList()); // Run synchronously on background thread
        foreach (var comment in comments)
        {
            Console.WriteLine($"{comment.Id}: {comment.Body} on Post {comment.PostId} by User {comment.UserId}");
        }
    }
}
