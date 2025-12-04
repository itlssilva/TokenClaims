using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TokenClaimConsumer.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            // Estes valores devem corresponder aos utilizados na Aplicação que emitiu o JWT
            ValidIssuer = "TokenClaimProvider",
            ValidAudience = "TokenClaimConsumer",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("CRIAR_CHAVE_SECRETA_MUITO_LONGA_AQUI"))
        };
    });

builder.Services.AddAuthorization();
builder.AddClaimsPolicies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.WebRoutes();

app.Run();