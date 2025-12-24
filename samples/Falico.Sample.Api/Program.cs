using Falico;
using Falico.Sample.Api.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add OpenAPI support
builder.Services.AddOpenApi();

// Register Falico mediator and handlers
builder.Services.AddFalico(typeof(Program));

// Register repository as singleton (in-memory storage)
builder.Services.AddSingleton<UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Falico Sample API";
        options.Theme = ScalarTheme.DeepSpace;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Logger.LogInformation("🚀 Falico Sample API is running!");
app.Logger.LogInformation("📚 API Documentation available at: /scalar/v1");

app.Run();
