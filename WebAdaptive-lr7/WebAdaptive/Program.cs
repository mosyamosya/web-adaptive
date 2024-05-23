using WebAdaptive.Services;
using WebAdaptive.Services.ApiService;
using WebAdaptive.Services.CommentService;
using WebAdaptive.Services.SeriesService;
using WebAdaptive.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
// Register your service and interface
builder.Services
    .AddSingleton<IApiService, ApiService>() //it's stateless and can be shared across the application
    .AddSingleton<ICommentService, CommentService>() //it's stateless and can be shared across the application
    .AddSingleton<IUserService, UserService>() //same as CommentService
    .AddSingleton<ISeriesService, SeriesService>(); //same as CommentService

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
