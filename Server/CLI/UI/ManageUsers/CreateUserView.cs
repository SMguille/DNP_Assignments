using System;
using RepositoryContracts;
using Entities;
namespace CLI.UI.ManageUsers;

public class CreateUserView(IUserRepository UserRepository)
{
    public async Task CreateUserAsync()
    {
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();
        Console.Write("Enter password: ");
        string? password = Console.ReadLine();
        Console.Write("Enter email: ");
        string? email = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Username, password, and email cannot be empty.");
            return;
        }

        User newUser = new User(username, password, email);
        await UserRepository.AddAsync(newUser);
        Console.WriteLine($"User '{username}' created with ID {newUser.Id}.");
    }
}
