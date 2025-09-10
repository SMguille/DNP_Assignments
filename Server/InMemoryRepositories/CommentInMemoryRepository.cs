using System;
using RepositoryContracts;
using Entities;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private readonly List<Comment> _comments = [];

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = _comments.Any() 
            ? _comments.Max(c => c.Id) + 1
            : 1;
        _comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = _comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
        }

        _comments.Remove(existingComment);
        _comments.Add(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = _comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        _comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = _comments.SingleOrDefault(c => c.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return _comments.AsQueryable();
    }

    public void Update(Comment comment)
    {
        throw new NotImplementedException();
    }
}