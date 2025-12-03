namespace TokenClaimProvider.Api;

public interface IGenerateToken
{
    JwtResponse GetToken(string username);
}