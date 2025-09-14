using System;
using System.Runtime.CompilerServices;
using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using Entities;
using InMemoryRepositories;
using RepositoryContracts;

public class CliApp
{
    public CliApp(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
    {
        PostRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        CommentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
    }

    private IPostRepository PostRepository { get; }
    private IUserRepository UserRepository { get; }
    private ICommentRepository CommentRepository { get; }

    internal async Task StartAsync()
    {
        Console.WriteLine("The following commands are available:");
        Console.WriteLine(" exit: Exit the application");
        Console.WriteLine(" users: See all users");
        Console.WriteLine(" createUser: Create a new user");
        Console.WriteLine(" posts: See all posts");
        Console.WriteLine(" viewPost: View a specific post");
        Console.WriteLine(" createPost: Create a new post");
        Console.WriteLine(" comments: See all comments");
        Console.WriteLine(" addComment: Add a new comment to a post");
        Console.WriteLine(" help: Show this help message");

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (input is null)
            {
                continue;
            }

            else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            else if (input.Equals("users", StringComparison.OrdinalIgnoreCase))
            {
                ListUsersView listUsersView = new ListUsersView(UserRepository);
                await listUsersView.ListUsersAsync();
            }
            else if (input.Equals("createUser", StringComparison.OrdinalIgnoreCase))
            {
                CreateUserView createUserView = new CreateUserView(UserRepository);
                await createUserView.CreateUserAsync();
            }
            else if (input.Equals("posts", StringComparison.OrdinalIgnoreCase))
            {
                ListPostsView listPostsView = new ListPostsView(PostRepository);
                await listPostsView.ListPostsAsync();
            }
            else if (input.Equals("viewPost", StringComparison.OrdinalIgnoreCase))
            {
                SinglePostView singlePostView = new SinglePostView(PostRepository);
                await singlePostView.ViewPostAsync();
            }
            else if (input.Equals("createPost", StringComparison.OrdinalIgnoreCase))
            {
                CreatePostView createPostView = new CreatePostView(PostRepository);
                await createPostView.CreateAsync();
            }
            else if (input.Equals("comments", StringComparison.OrdinalIgnoreCase))
            {
                ListCommentsView listCommentsView = new ListCommentsView(CommentRepository);
                await listCommentsView.ListCommentsAsync();
            }
            else if (input.Equals("addComment", StringComparison.OrdinalIgnoreCase))
            {
                CreateCommentView createCommentView = new CreateCommentView(CommentRepository, PostRepository, UserRepository);
                await createCommentView.CreateCommentAsync();
            }
            else if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("The following commands are available:");
                Console.WriteLine(" exit: Exit the application");
                Console.WriteLine(" users: See all users");
                Console.WriteLine(" createUser: Create a new user");
                Console.WriteLine(" posts: See all posts");
                Console.WriteLine(" viewPost: View a specific post");
                Console.WriteLine(" createPost: Create a new post");
                Console.WriteLine(" comments: See all comments");
                Console.WriteLine(" addComment: Add a new comment to a post");
                Console.WriteLine(" help: Show this help message");
            }
            Console.WriteLine($"You entered: {input}");
        }
        Console.WriteLine("CLI App exiting...");
    }
}