using System;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView(IUserRepository UserRepository)
{
    public async Task ListUsersAsync()
    {
        Console.WriteLine("Listing users...");
        var usersQuery = UserRepository.GetMany();
        var users = await Task.Run(() => usersQuery.ToList()); // Run synchronously on background thread
        foreach (var user in users)
        {
            Console.WriteLine($"id: {user.Id}: name: {user.Username} email: {user.Email}");
        }
    }
}
