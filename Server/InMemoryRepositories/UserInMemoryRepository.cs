using System;
using RepositoryContracts;
using Entities;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public UserInMemoryRepository()
    {
        AddAsync(new User("Alice Smith", "123456", "alice@example.com"));
        AddAsync(new User("Bob Johnson", "abcdef", "bob@example.com"));
        AddAsync(new User("Charlie Brown", "qwerty", "charlie@example.com"));
        AddAsync(new User("Diana Prince", "zxcvbn", "diana@example.com"));
        AddAsync(new User("Ethan Hunt", "passw0rd", "ethan@example.com"));
    }

    public Task<User> AddAsync(User user)
    {
        user.Id = _users.Any()
            ? _users.Max(u => u.Id) + 1
            : 1;
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = _users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException($"User with ID '{user.Id}' not found");
        }

        _users.Remove(existingUser);
        _users.Add(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = _users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }

        _users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? user = _users.SingleOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }
        return Task.FromResult(user);
    }

    public IQueryable<User> GetManyAsync()
    {
        return _users.AsQueryable();
    }
}
