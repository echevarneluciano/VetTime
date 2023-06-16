using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using VetTime.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder
                .WithOrigins("http://localhost:5200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
    });


builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = "https://dev-imbas1v3lp0rrnnp.us.auth0.com/";
            options.Audience = "http://localhost:5200/";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = ClaimTypes.NameIdentifier
            };
        });




/* builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddJwtBearer(options =>//la api web valida con token
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["TokenAuthentication:Issuer"],
            ValidAudience = configuration["TokenAuthentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                configuration["TokenAuthentication:SecretKey"])),//
        };
        // opción extra para usar el token en el hub y otras peticiones sin encabezado (enlaces, src de img, etc.)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Leer el token desde el query string
                var accessToken = context.Request.Query["access_token"];
                // Si el request es para el Hub u otra ruta seleccionada...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/api/usuarios/reset") ||
                    path.StartsWithSegments("/api/usuarios/token")))
                {//reemplazar las urls por las necesarias ruta ⬆
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
*/

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador", "SuperAdministrador"));
});

builder.Services.AddDbContext<DataContext>(
    options => options.UseMySql(
        configuration["ConnectionStrings:MySql"],
        ServerVersion.AutoDetect(configuration["ConnectionStrings:MySql"])
    )
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

builder.WebHost.UseUrls("http://localhost:5200", "http://*:5200");//, "http://192.168.15.31:5200"

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseStaticFiles();

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
