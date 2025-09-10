using System;
using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> AddAsync(User user)
    {
        return Task.FromResult(user);
    }
    public void Update(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));
    }
    Task DeleteAsync(int id);
    Task<User> GetSingleAsync(int id);
    IQueryable<User> GetManyAsync();

}
