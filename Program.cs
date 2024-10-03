using Microsoft.EntityFrameworkCore;
using MINIMALAPI.Domain.DTOS;
using MINIMALAPI.Infrastructure.Db; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ContextDb>( options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"), 
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql")));
});

var app = builder.Build();

app.MapGet("/", () => "Olá, Admirável Mundo Novo!");
app.MapPost("/login", (LoginDTO loginDTO) => {
    if (loginDTO.Email == "admin@admin.com" && loginDTO.Senha == "admin")
        return Results.Ok();
    else
        return Results.Unauthorized();
});


app.Run();