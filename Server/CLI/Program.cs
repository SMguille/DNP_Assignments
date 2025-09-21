using System;
using FileRepositories;
using RepositoryContracts;


Console.WriteLine("Starting CLI App...");
IPostRepository postRepository = new PostFileRepository();
IUserRepository userRepository = new UserFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();

CliApp cliApp = new CliApp(postRepository, userRepository, commentRepository);
await cliApp.StartAsync();