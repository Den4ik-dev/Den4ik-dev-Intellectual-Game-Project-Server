using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using server.Application.Interfaces;

namespace server.Infrastructure.Services;

public class JwtTokenService : ITokenService
{
    private IConfiguration _config;

    public JwtTokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        JwtSecurityToken jwt = new JwtSecurityToken(
            issuer: _config["JWT:ISSUER"],
            audience: _config["JWT:AUDIENCE"],
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:KEY"])),
                SecurityAlgorithms.HmacSha256
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public string GenerateRefreshToken()
    {
        byte[] randomNumbers = new byte[32];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumbers);
            return Convert.ToBase64String(randomNumbers);
        }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = _config["JWT:ISSUER"],
            ValidAudience = _config["JWT:AUDIENCE"],
            ValidateIssuer = true,
            ValidateAudience = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:KEY"])),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        ClaimsPrincipal principal = tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out SecurityToken securityToken
        );

        if (
            securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase
            )
        )
        {
            throw new InvalidOperationException("invalid token");
        }

        return principal;
    }
}
