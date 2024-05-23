using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithMetrics(opt =>
    
        opt
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Series.GatewayAPI"))
            .AddMeter(builder.Configuration.GetValue<string>("SeriesMeterName"))
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddOtlpExporter(opts =>
            {
                opts.Endpoint = new Uri(builder.Configuration["Otel:Endpoint"]);
            })
    );   


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Fantasy", "Action", "Horror", "Comedy", "Adventure","Romance","Drama", "Mystery"
};

app.MapGet("/category", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new CategoryData
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetCategory")
.WithOpenApi();



app.Run();

record CategoryData(DateOnly Date, string? Summary);