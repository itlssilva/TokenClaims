using System.Security.Claims;

namespace TokenClaimConsumer.Api;

public static class ClaimsPolicies
{
    public static WebApplicationBuilder AddClaimsPolicies(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminClaim", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Admin"))
            .AddPolicy("RequireOperatorClaim", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Operator"));

        return builder;
    }
}