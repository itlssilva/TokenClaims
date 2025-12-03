using System.Security.Claims;

namespace TokenClaimConsumer.Api;

public static class Routes
{
    public static WebApplication WebRoutes(this WebApplication app)
    {
        app.MapGet("/verifyClaims", (ClaimsPrincipal user) =>
            {

                // 1. Verificando a presença de Claims
                if (user.Identity is not { IsAuthenticated: true })
                {
                    // Isso não deve acontecer se o [Authorize] funcionar, mas é um bom fallback
                    return Results.BadRequest("Token inválido ou não autenticado.");
                }

                // 2. Extraindo as Claims
                var userClaims = user.Claims.Select(claim => new UserClaimInfo
                    {
                        Type = claim.Type,
                        Value = claim.Value
                    })
                    .ToList();

                // 3. Verificando Claims Específicas para Autorização
                string? username = user.FindFirst(ClaimTypes.Name)?.Value;
                string? role = user.FindFirst(ClaimTypes.Role)?.Value;

                return Results.Ok(new
                {
                    Username = username,
                    Role = role,
                    AllClaims = userClaims
                });
            })
            .RequireAuthorization();

        app.MapGet("/NoClaims", () => "EndPoint sem autenticação necessária");

        return app;
    }
}