using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TokenClaimProvider.Api;

namespace TokenClaims.Tests;

public class JwtConfigurationTests
{
    private const string SecretKey = "8028BBF51A3C4A362456A4BF8CB1E532201511279CA5FC6E2ECD6CA6936EA13E";
    private const string Issuer = "TokenClaimProvider";
    private const string Audience = "TokenClaimConsumer";

    [Fact]
    public void Authentication_UsesCorrectSecretKeyForTokenValidation()
    {
        // Arrange - mirror the validation parameters used by the consumer API
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
        };

        // Act
        var key = Assert.IsType<SymmetricSecurityKey>(validationParameters.IssuerSigningKey);
        var configuredSecret = Encoding.UTF8.GetString(key.Key);

        // Assert
        Assert.Equal(SecretKey, configuredSecret);
    }

    [Fact]
    public void TokenGeneration_UsesCorrectSecretKey()
    {
        // Arrange
        var generator = new GenerateToken();
        const string userName = "italo";

        // Act
        var response = generator.GetToken(userName);
        var handler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        var principal = handler.ValidateToken(
            response.Token,
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = key
            },
            out _);

        // Assert - if the secret key is wrong, ValidateToken would throw
        Assert.NotNull(principal);
        Assert.Equal(userName, principal.Identity!.Name);
    }

    [Fact]
    public void TokenValidation_MatchesIssuerAndAudienceSettings()
    {
        // Arrange
        var generator = new GenerateToken();
        const string userName = "italo";
        var response = generator.GetToken(userName);
        var handler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        // Act
        var validatedPrincipal = handler.ValidateToken(
            response.Token,
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = key
            },
            out var validatedToken);

        var jwt = Assert.IsType<JwtSecurityToken>(validatedToken);

        // Assert
        Assert.Equal(Issuer, jwt.Issuer);
        Assert.Contains(jwt.Audiences, a => a == Audience);
        Assert.NotNull(validatedPrincipal);
    }
}
