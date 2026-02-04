using TokenClaimProvider.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<IGenerateToken, GenerateToken>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapPost("/login/{userName}", (IGenerateToken generateToken, string userName) =>
{
    var token = generateToken.GetToken(userName);
    return Results.Ok(token);
});

app.Run();
