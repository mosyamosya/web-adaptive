using Microsoft.OpenApi.Models;
using WebAdaptive.Services;
using WebAdaptive.Services.ApiService;
using WebAdaptive.Services.CommentService;
using WebAdaptive.Services.SeriesService;
using WebAdaptive.Services.UserService;
using WebAdaptive.Services.AuthService;
using WebAdaptive.Services.DataRandomizerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Builder;
using WebAdaptive.Services.HealthCheck;
using WebAdaptive.Services.DBService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

var versions = new[] { "v1.1", "v1.2", "v1.3", "v2.1" };

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

// Register your service and interface
builder.Services
    .AddSingleton<IApiService, ApiService>() //it's stateless and can be shared across the application
    .AddSingleton<ICommentService, CommentService>() //it's stateless and can be shared across the application
    .AddSingleton<IUserService, UserService>() //same as CommentService
    .AddSingleton<ISeriesService, SeriesService>()//same as CommentService
    .AddSingleton<IAuthService, AuthService>()
    .AddTransient<IDataRandomizerService, DataRandomizerService>();

builder.Services.AddDbContext<DBService>(options => options
           .UseNpgsql(
                   builder.Configuration.GetConnectionString("DefaultConnection")
                   ));



builder.Services.AddHealthChecks()
                .AddTypeActivatedCheck<UserHealthCheck>("user_health_check", args: new object[] { "Token type - JWT Bearer" })
                .AddDbContextCheck<DBService>("database_health_check");

builder.Services.AddHealthChecksUI(
    o =>
    {
        o.AddHealthCheckEndpoint("user_health_check", "/user_health");
        o.AddHealthCheckEndpoint("database_health_check", "/database_health");
    }).AddInMemoryStorage();



builder.Services.AddSwaggerGen(c =>
{
    foreach (var version in versions)
    {
        c.SwaggerDoc($"{version}", new OpenApiInfo { Title = "Series API", Version = $"{version}", });
    }

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization (Bearer scheme)",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAuthentication(optins =>
{
    optins.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    optins.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(2, 1);
})
.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        foreach (var version in versions)
        {
            c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"Series API {version}");
        }
    });
}

app.MapHealthChecks("/user_health",
                new HealthCheckOptions
                {
                    Predicate = healthCheck => healthCheck.Name == "dictionary_health_check",
                    AllowCachingResponses = false,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

app.MapHealthChecks("/database_health",
    new HealthCheckOptions
    {
        Predicate = healthCheck => healthCheck.Name == "database_health_check",
        AllowCachingResponses = false,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.UseHealthChecksUI(opt =>
{
    opt.UIPath = "/health";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
