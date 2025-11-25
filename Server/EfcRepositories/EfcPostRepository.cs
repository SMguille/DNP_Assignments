using System;
using Entities;
using RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext ctx;

    public EfcPostRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Post> AddAsync(Post post)
    {
        await ctx.Posts.AddAsync(post);
        await ctx.SaveChangesAsync();
        return post;
    }

    public async Task<Post> UpdateAsync(Post post)
    {
        if (!(await ctx.Posts.AnyAsync(p => p.Id == post.Id)))
        {
            throw new NotFoundException($"Post with id {post.Id} not found");
        }

        ctx.Posts.Update(post);
        await ctx.SaveChangesAsync();

        return post;
    }
    public async Task DeleteAsync(int id)
    {
        Post? existing = await ctx.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            throw new NotFoundException($"Post with id {id} not found");
        }

        ctx.Posts.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        Post? existing = await ctx.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            throw new NotFoundException($"Post with id {id} not found");
        }
        return existing;
    }

    public IQueryable<Post> GetMany()
    {
        return ctx.Posts.AsQueryable();
    }
}