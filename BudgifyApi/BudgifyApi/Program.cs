using BudgifyApi.Data;
using Microsoft.EntityFrameworkCore;
using BudgifyApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<UsersDB>(options => 
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/users/", async (Usuario u, UsersDB db) =>
{
    db.usuarios.Add(u);
    await db.SaveChangesAsync();

    return Results.Created("$/user/{u.Id}", u);
});

app.MapGet("/users/{id:int}", async (int id, UsersDB db) =>
{
    return await db.usuarios.FindAsync(id)
        is Usuario u
        ? Results.Ok(u)
        : Results.NotFound();
});

app.MapGet("/users", async (UsersDB db) => await db.usuarios.ToListAsync());

app.MapPut("/users/{id:int}", async (int id, Usuario e, UsersDB db)=>
{
    if (e.Id != id)
        return Results.BadRequest();

    var user = await db.usuarios.FindAsync(id);

    if (user is null) return Results.NotFound();

    user.Nombre = e.Nombre;
    user.Apellido = e.Apellido;
    user.Email = e.Email;
    user.Contrase�a = e.Contrase�a;

    await db.SaveChangesAsync();

    return Results.Ok(user);
}
);

app.MapDelete("/users/{id:int}", async (int id, UsersDB db) => 
{
    var user = await db.usuarios.FindAsync(id);

    if (user is null) return Results.NotFound();

    db.usuarios.Remove(user);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/applicationName/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "info/swagger";
});

app.Run();
