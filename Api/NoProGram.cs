using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MINIMALAPI.Domain.DTOS;
using MINIMALAPI.Domain.Entities;
using MINIMALAPI.Domain.Enums;
using MINIMALAPI.Domain.Interfaces;
using MINIMALAPI.Domain.ModelViews;
using MINIMALAPI.Domain.Services;
using MINIMALAPI.Infrastructure.Db; 

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();

if(string.IsNullOrEmpty(key))
    key = "123456";

builder.Services.AddAuthentication(option => {
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option => {
    option.TokenValidationParameters = new TokenValidationParameters{
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

# region Builder
//Services
builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();
//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o Toker JWT neste formato: Bearer {token}."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<ContextDb>( options => {
    options.UseMySql(
        builder.Configuration.GetConnectionString("Mysql"), 
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Mysql")));
});

var app = builder.Build();
# endregion

# region Home
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
# endregion

# region Administradores
string GenerateTokenJWT(Administrator administrator)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
       new Claim("Email", administrator.Email),
       new Claim("Perfil", administrator.Perfil),
       new Claim(ClaimTypes.Role, administrator.Perfil)
    };
    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
    return tokenString;
};

app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministratorService administratorService) => {
    var adm = administratorService.Login(loginDTO);
    if (adm != null)
    {
        string toked = GenerateTokenJWT(adm);
         return Results.Ok(new AdminLogadoModelView
         {
            Email = adm.Email,
            Perfil = adm.Perfil,
            Token = toked
         });       
    }
    else
    {
        return Results.Unauthorized();
    }
    
}).AllowAnonymous().WithTags("Administradores");

app.MapPost("/administrador", ([FromBody] AdministradorDTO administradorDTO, IAdministratorService administratorService) => {
    var validacao = new ValidationErrors{
        Mensagens = new List<string>()
    };

    if(string.IsNullOrEmpty(administradorDTO.Email))
        validacao.Mensagens.Add("O Email tem de ser preenchido.");

    if(string.IsNullOrEmpty(administradorDTO.Senha))
        validacao.Mensagens.Add("Uma senha tem de ser inserida.");

    if(administradorDTO.Perfil == null)
        validacao.Mensagens.Add("Um perfil tem de ser dado ao administrador.");

    if(validacao.Mensagens.Count() > 0)
        return Results.BadRequest(validacao);

    var administrator = new Administrator
    {
        Email = administradorDTO.Email,
        Senha = administradorDTO.Senha,
        Perfil = administradorDTO.Perfil.ToString() ?? PerfilEnum.Editor.ToString()
    };

    administratorService.Incluir(administrator);

    return Results.Created($"/administradores/{administrator.Id}", new AdministradorModelView{
            Id = administrator.Id,
            Email = administrator.Email,
            Perfil = administrator.Perfil
        });
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm" })
.WithTags("Administradores");

app.MapGet("/administradores", ([FromQuery] int? pagina, IAdministratorService administratorService) => {
    var adms = new List<AdministradorModelView>();
    var administradores = administratorService.Todos(pagina);
    foreach (var adm in administradores)
    {
        adms.Add(new AdministradorModelView{
            Id = adm.Id,
            Email = adm.Email,
            Perfil = adm.Perfil
        });
    }
    return Results.Ok(adms);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm" })
.WithTags("Administradores");

app.MapGet("/administrador/{id}", ([FromRoute] int id, IAdministratorService administratorService) => {
    var administrador = administratorService.BuscaPorId(id);
    
    if(administrador == null) return Results.NotFound();
    
    return Results.Ok(new AdministradorModelView{
            Id = administrador.Id,
            Email = administrador.Email,
            Perfil = administrador.Perfil
        });
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm" })
.WithTags("Administradores");
# endregion

# region Veiculos
static ValidationErrors ValidaDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ValidationErrors{
        Mensagens = new List<string>()
    };

    if(string.IsNullOrEmpty(veiculoDTO.Nome))
        validacao.Mensagens.Add("O nome não pode ser vazio.");

    if(string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagens.Add("O veículo te de ter uma marca.");

    if(veiculoDTO.Ano < 1940)
        validacao.Mensagens.Add("Veículo muito antigo. Somente modelos a partir de 1940.");

    return validacao;
}


app.MapPost("/veiculo", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoService veiculoService) => {

    var validacao = ValidaDTO(veiculoDTO);
    if(validacao.Mensagens.Count() > 0)
        return Results.BadRequest(validacao);

    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };
    veiculoService.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm,Editor" })
.WithTags("Veículos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculoService veiculoService) => {
    var veiculos = veiculoService.Todos(pagina);
    return Results.Ok(veiculos);
}).RequireAuthorization().WithTags("Veículos");

app.MapGet("/veiculo/{id}", ([FromRoute] int id, IVeiculoService veiculoService) => {
    var veiculo = veiculoService.BuscaPorId(id);
    
    if(veiculo == null) return Results.NotFound();
    
    return Results.Ok(veiculo);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm,Editor" })
.WithTags("Veículos");


app.MapPut("/veiculo/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoService veiculoService) =>{
    
    var veiculo = veiculoService.BuscaPorId(id);
    if(veiculo == null) return Results.NotFound();

    var validacao = ValidaDTO(veiculoDTO);
    if(validacao.Mensagens.Count() > 0)
        return Results.BadRequest(validacao);

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    veiculoService.Atualizar(veiculo);
    
    return Results.Ok(veiculo);
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm" })
.WithTags("Veículos");

app.MapDelete("/veiculo/{id}", ([FromRoute] int id, IVeiculoService veiculoService) =>{
    
    var veiculo = veiculoService.BuscaPorId(id);
    if(veiculo == null) return Results.NotFound();

    veiculoService.Apagar(veiculo);
    
    return Results.NoContent();
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm" })
.WithTags("Veículos");
# endregion

# region app
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
# endregion