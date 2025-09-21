using System;
using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private readonly List<Post> _posts = new();
    
    public PostInMemoryRepository()
    {
        AddAsync(new Post("First Post", "This is the first dummy post.", 1));
        AddAsync(new Post("Second Post", "This is the second dummy post.", 2));
        AddAsync(new Post("Third Post", "This is the third dummy post.", 3));
        AddAsync(new Post("Fourth Post", "This is the fourth dummy post.", 4));
        AddAsync(new Post("Fifth Post", "This is the fifth dummy post.", 5));
    }

    public Task<Post> AddAsync(Post post)
    {
        post.Id = _posts.Any() 
            ? _posts.Max(p => p.Id) + 1
            : 1;
        _posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = _posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
        }

        _posts.Remove(existingPost);
        _posts.Add(post);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = _posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        _posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = _posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetMany()
    {
        return _posts.AsQueryable();
    }
}
