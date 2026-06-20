using ApiDocGen.Data.Entities;

namespace ApiDocGen.Services.Auth;

public record TokenPair(string AccessToken, string RefreshToken, DateTime RefreshTokenExpiry);

public interface ITokenService
{
    TokenPair GenerateTokenPair(User user);
    Guid? ValidateRefreshToken(string refreshToken);
    string HashToken(string token);
}
