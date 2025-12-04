using System.Security.Claims;

namespace TokenClaimConsumer.Api;

public static class ClaimsPolicies
{
    public static WebApplicationBuilder AddClaimsPolicies(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminClaim", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Admin"));

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("RequireOperatorClaim", policy =>
                policy.RequireClaim(ClaimTypes.Role, "Operator"));

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminOrOperator", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(ClaimTypes.Role, "Admin") ||
                    context.User.HasClaim(ClaimTypes.Role, "Operator")
                ));

        return builder;
    }
}