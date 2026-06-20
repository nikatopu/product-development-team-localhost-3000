using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiDocGen.Data.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ApiDocGen.Services.Auth;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config) => _config = config;

    public TokenPair GenerateTokenPair(User user)
    {
        var accessToken = GenerateAccessToken(user);
        var (refreshToken, expiry) = GenerateRefreshToken();
        return new TokenPair(accessToken, refreshToken, expiry);
    }

    public Guid? ValidateRefreshToken(string refreshToken)
    {
        // Caller looks up hashed token in DB and validates expiry/revocation
        // This method just decodes the userId embedded in the token
        try
        {
            var bytes = Convert.FromBase64String(refreshToken);
            if (bytes.Length < 16) return null;
            return new Guid(bytes[..16]);
        }
        catch
        {
            return null;
        }
    }

    public string HashToken(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

    private string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret not configured")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim("github_id", user.GithubId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"] ?? "driftless",
            audience: _config["Jwt:Audience"] ?? "driftless",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private (string token, DateTime expiry) GenerateRefreshToken()
    {
        var userId = Guid.NewGuid();
        var random = RandomNumberGenerator.GetBytes(32);
        var combined = userId.ToByteArray().Concat(random).ToArray();
        var token = Convert.ToBase64String(combined);
        var expiry = DateTime.UtcNow.AddDays(30);
        return (token, expiry);
    }
}
