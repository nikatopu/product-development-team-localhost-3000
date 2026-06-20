using ApiDocGen.Data;
using ApiDocGen.Data.Entities;
using ApiDocGen.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ApiDocGen.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ApplicationDbContext db,
        ITokenService tokenService,
        IHttpClientFactory httpClientFactory,
        IConfiguration config,
        ILogger<AuthController> logger)
    {
        _db = db;
        _tokenService = tokenService;
        _httpClientFactory = httpClientFactory;
        _config = config;
        _logger = logger;
    }

    /// <summary>Redirect user to GitHub OAuth login</summary>
    [HttpGet("github/login")]
    public IActionResult GitHubLogin([FromQuery] string? returnUrl = null)
    {
        var clientId = _config["GitHub:ClientId"]
            ?? throw new InvalidOperationException("GitHub:ClientId not configured");
        var state = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var callbackUrl = _config["GitHub:CallbackUrl"] ?? "http://localhost:5141/api/auth/github/callback";

        var githubUrl = $"https://github.com/login/oauth/authorize" +
            $"?client_id={clientId}" +
            $"&redirect_uri={Uri.EscapeDataString(callbackUrl)}" +
            $"&scope=read:user,user:email,repo" +
            $"&state={state}";

        return Redirect(githubUrl);
    }

    /// <summary>GitHub OAuth callback — exchange code for tokens and redirect to frontend</summary>
    [HttpGet("github/callback")]
    public async Task<IActionResult> GitHubCallback(
        [FromQuery] string code, [FromQuery] string state)
    {
        var clientId = _config["GitHub:ClientId"]!;
        var clientSecret = _config["GitHub:ClientSecret"]!;
        var frontendUrl = _config["Frontend:Url"] ?? "http://localhost:5173";

        try
        {
            // Exchange code for GitHub access token
            var http = _httpClientFactory.CreateClient();
            http.DefaultRequestHeaders.Add("Accept", "application/json");
            http.DefaultRequestHeaders.Add("User-Agent", "Driftless");

            var tokenResponse = await http.PostAsJsonAsync(
                "https://github.com/login/oauth/access_token",
                new { client_id = clientId, client_secret = clientSecret, code });

            var tokenJson = await tokenResponse.Content.ReadFromJsonAsync<JsonElement>();
            if (!tokenJson.TryGetProperty("access_token", out var accessTokenEl))
                return Redirect($"{frontendUrl}/auth/error?message=github_token_failed");

            var githubToken = accessTokenEl.GetString()!;

            // Fetch GitHub user profile
            http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", githubToken);

            var userResponse = await http.GetFromJsonAsync<JsonElement>("https://api.github.com/user");
            var githubId = userResponse.GetProperty("id").GetInt64();
            var username = userResponse.GetProperty("login").GetString()!;
            var avatarUrl = userResponse.GetProperty("avatar_url").GetString()!;
            var email = userResponse.TryGetProperty("email", out var emailEl)
                ? emailEl.GetString() : null;

            // Upsert user
            var user = await _db.Users.FirstOrDefaultAsync(u => u.GithubId == githubId);
            if (user == null)
            {
                user = new User { GithubId = githubId };
                _db.Users.Add(user);
            }

            user.Username = username;
            user.AvatarUrl = avatarUrl;
            user.Email = email;
            user.GithubAccessToken = githubToken;
            await _db.SaveChangesAsync();

            // Generate JWT + refresh token
            var tokens = _tokenService.GenerateTokenPair(user);
            var refreshHash = _tokenService.HashToken(tokens.RefreshToken);

            _db.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshHash,
                ExpiresAt = tokens.RefreshTokenExpiry,
            });
            await _db.SaveChangesAsync();

            // Redirect to frontend with tokens in query params (frontend stores in localStorage)
            return Redirect(
                $"{frontendUrl}/auth/callback" +
                $"?access_token={Uri.EscapeDataString(tokens.AccessToken)}" +
                $"&refresh_token={Uri.EscapeDataString(tokens.RefreshToken)}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GitHub OAuth callback failed");
            return Redirect($"{frontendUrl}/auth/error?message=internal_error");
        }
    }

    /// <summary>Exchange a refresh token for a new access token</summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var hash = _tokenService.HashToken(request.RefreshToken);
        var token = await _db.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.TokenHash == hash && !t.IsRevoked);

        if (token == null || token.ExpiresAt < DateTime.UtcNow)
            return Unauthorized("Invalid or expired refresh token.");

        var newTokens = _tokenService.GenerateTokenPair(token.User);
        var newHash = _tokenService.HashToken(newTokens.RefreshToken);

        // Rotate: revoke old, create new
        token.IsRevoked = true;
        token.RevokedReason = "rotated";
        _db.RefreshTokens.Add(new RefreshToken
        {
            UserId = token.UserId,
            TokenHash = newHash,
            ExpiresAt = newTokens.RefreshTokenExpiry,
        });
        await _db.SaveChangesAsync();

        return Ok(new { accessToken = newTokens.AccessToken, refreshToken = newTokens.RefreshToken });
    }

    /// <summary>Revoke refresh token (logout)</summary>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
    {
        var hash = _tokenService.HashToken(request.RefreshToken);
        var token = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == hash);
        if (token != null)
        {
            token.IsRevoked = true;
            token.RevokedReason = "logout";
            await _db.SaveChangesAsync();
        }
        return NoContent();
    }

    /// <summary>Get current authenticated user</summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(userId, out var id)) return Unauthorized();

        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();

        return Ok(new
        {
            id = user.Id,
            githubId = user.GithubId,
            username = user.Username,
            avatarUrl = user.AvatarUrl,
            email = user.Email,
        });
    }
}

public record RefreshRequest(string RefreshToken);
