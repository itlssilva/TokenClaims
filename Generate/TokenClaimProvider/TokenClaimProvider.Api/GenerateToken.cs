using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TokenClaimProvider.Api;

public class GenerateToken : IGenerateToken
{
    private const string SecretKey = "CRIAR_CHAVE_SECRETA_MUITO_LONGA_AQUI";
    private const string Issuer = "TokenClaimProvider";
    private const string Audience = "TokenClaimConsumer";

    public JwtResponse GetToken(string username)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, username == "italo" ?  "Admin" : "Operator"),
            // ,
            // new Claim("PermissaoEspecifica", "CriarRelatorio")
        };

        // 3. Configuração da Chave de Assinatura
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 4. Definição da Expiração
        var expires = DateTime.Now.AddMinutes(30);

        // 5. Criação do Token Descriptor
        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        // 6. Geração e Retorno do Token
        string? tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new JwtResponse
        {
            Token = tokenString,
            Expiration = expires
        };
    }
}