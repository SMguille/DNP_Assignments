using System;
using InMemoryRepositories;
using RepositoryContracts;


Console.WriteLine("Starting CLI App...");
IPostRepository postRepository = new PostInMemoryRepository();
IUserRepository userRepository = new UserInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();

CliApp cliApp = new CliApp(postRepository, userRepository, commentRepository);
await cliApp.StartAsync();