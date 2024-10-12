using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MINIMALAPI.Domain.DTOS;
using MINIMALAPI.Domain.Entities;
using MINIMALAPI.Domain.Interfaces;
using MINIMALAPI.Domain.ModelViews;
using MINIMALAPI.Domain.Services;
using MINIMALAPI.Infrastructure.Db; 

var builder = WebApplication.CreateBuilder(args);

//# region Builder
//Services
builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();
//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContextDb>( options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"), 
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql")));
});

var app = builder.Build();
//# end region
//# region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
//# end region Home
//# region Administradores
app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministratorService administratorService) => {
    if (administratorService.Login(loginDTO) != null)
        return Results.Ok();
    else
        return Results.Unauthorized();
}).WithTags("Administradores");
//# end region
//# region Veiculos
app.MapPost("/veiculo", ([FromBody] VeiculoDTO VeiculoDTO, IVeiculoService veiculoService) => {
    var veiculo = new Veiculo
    {
        Nome = VeiculoDTO.Nome,
        Marca = VeiculoDTO.Marca,
        Ano = VeiculoDTO.Ano
    };
    veiculoService.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).WithTags("Veículos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculoService veiculoService) => {
    var veiculos = veiculoService.Todos(pagina);
    return Results.Ok(veiculos);
}).WithTags("Veículos");

app.MapGet("/veiculo/{id}", ([FromRoute] int id, IVeiculoService veiculoService) => {
    var veiculo = veiculoService.BuscaPorId(id);
    
    if(veiculo == null) return Results.NotFound();
    
    return Results.Ok(veiculo);
}).WithTags("Veículos");


app.MapPut("/veiculo/{id}", ([FromRoute] int id, VeiculoDTO VeiculoDTO, IVeiculoService veiculoService) =>{
    
    var veiculo = veiculoService.BuscaPorId(id);
    if(veiculo == null) return Results.NotFound();

    veiculo.Nome = VeiculoDTO.Nome;
    veiculo.Marca = VeiculoDTO.Marca;
    veiculo.Ano = VeiculoDTO.Ano;

    veiculoService.Atualizar(veiculo);
    
    return Results.Ok(veiculo);
}).WithTags("Veículos");

app.MapDelete("/veiculo/{id}", ([FromRoute] int id, IVeiculoService veiculoService) =>{
    
    var veiculo = veiculoService.BuscaPorId(id);
    if(veiculo == null) return Results.NotFound();

    veiculoService.Apagar(veiculo);
    
    return Results.NoContent();
}).WithTags("Veículos");
//# end region
//# region app
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
//# end region