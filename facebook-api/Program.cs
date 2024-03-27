using facebook_api.Database;
using facebook_api.Models.DTO.Request.Email;
using facebook_api.Repository.Comment;
using facebook_api.Repository.Friendship;
using facebook_api.Repository.Post;
using facebook_api.Repository.Reaction;
using facebook_api.Repository.User;
using facebook_api.Service.Comment;
using facebook_api.Service.Email;
using facebook_api.Service.File;
using facebook_api.Service.Friendship;
using facebook_api.Service.Hash;
using facebook_api.Service.Post;
using facebook_api.Service.Reaction;
using facebook_api.Service.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dependency injection
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFriendshipRepository, FriendshipRepository>();
builder.Services.AddScoped<IFriendshipService, FriendshipService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IReactionRepository, ReactionRepository>();
builder.Services.AddScoped<IReactionService, ReactionService>();
builder.Services.AddScoped<IPasswordHashing, PasswordHashing>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();

// options
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
