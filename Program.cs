using MINIMALAPI.Domain.DTOS;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Olá, Admirável Mundo Novo!");
app.MapPost("/login", (LoginDTO loginDTO) => {
    if (loginDTO.Email == "admin@admin.com" && loginDTO.Senha == "admin")
        return Results.Ok();
    else
        return Results.Unauthorized();
});


app.Run();