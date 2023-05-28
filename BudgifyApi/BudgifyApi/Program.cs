using BudgifyDal;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Configures the application services and pipeline.
/// </summary>
/// <param name="args">Command-line arguments.</param>
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// Gets the connection string for the PostgreSQL database.
/// </summary>
/// <remarks>
/// The connection string is retrieved from the configuration using the key "Postgres".
/// </remarks>
var connectionString = builder.Configuration.GetConnectionString("Postgres");

/// <summary>
/// Adds the AppDbContext to the service container.
/// </summary>
/// <remarks>
/// The AppDbContext is configured to use PostgreSQL with the provided connection string.
/// </remarks>
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

/// <summary>
/// Configures the CORS policy.
/// </summary>
/// <remarks>
/// Allows requests from any origin, with any header, and any method.
/// </remarks>
builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

/// <summary>
/// Configures Swagger and Swagger UI in the development environment.
/// </summary>
/// <remarks>
/// Adds the Swagger middleware and enables the Swagger UI.
/// </remarks>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/// <summary>
/// Redirects HTTP requests to HTTPS.
/// </summary>
/// <remarks>
/// This middleware ensures that all HTTP requests are redirected to HTTPS for secure communication.
/// </remarks>
app.UseHttpsRedirection();

/// <summary>
/// Applies the configured CORS policy.
/// </summary>
app.UseCors("NewPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
