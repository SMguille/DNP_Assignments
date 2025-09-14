using System;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView(IUserRepository UserRepository)
{
    public async Task ListUsersAsync()
    {
        Console.WriteLine("Listing users...");
        var usersQuery = UserRepository.GetManyAsync();
        var users = usersQuery.ToList(); // Synchronous
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}: {user.Username}");
        }
    }
}
