namespace TokenClaimProvider.Api;

public class JwtResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}