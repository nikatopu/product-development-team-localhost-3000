using System.Text;
using System.Threading.RateLimiting;
using ApiDocGen.Data;
using ApiDocGen.Hubs;
using ApiDocGen.Services;
using ApiDocGen.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers + Swagger ──────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Driftless API", Version = "v1" });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ApiDocGen.xml"), true);
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter your JWT access token",
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ── Database (PostgreSQL via EF Core) ──────────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddDbContext<ApplicationDbContext>(opts =>
        opts.UseNpgsql(connectionString));
}
else
{
    // Graceful degradation: register a no-op/in-memory context so controllers that
    // don't use the DB still resolve. Auth features will return 503 until a DB is
    // configured. Log a warning so it's visible in startup.
    builder.Services.AddDbContext<ApplicationDbContext>(opts =>
        opts.UseInMemoryDatabase("driftless-dev"));
    Console.WriteLine("WARNING: No database connection string configured. " +
        "Using in-memory database — data will not persist between restarts. " +
        "Set ConnectionStrings:DefaultConnection in appsettings or environment to use PostgreSQL.");
}

// ── JWT Authentication ─────────────────────────────────────────────────────
var jwtSecret = builder.Configuration["Jwt:Secret"];
if (!string.IsNullOrWhiteSpace(jwtSecret))
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "driftless",
                ValidAudience = builder.Configuration["Jwt:Audience"] ?? "driftless",
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSecret)),
            };
        });
    builder.Services.AddAuthorization();
}
else
{
    // No JWT secret — auth disabled, all [Authorize] endpoints return 401
    builder.Services.AddAuthentication();
    builder.Services.AddAuthorization();
    Console.WriteLine("WARNING: Jwt:Secret not configured. Authentication is disabled.");
}

// ── Core analysis services ─────────────────────────────────────────────────
builder.Services.AddScoped<IGitService, GitService>();
builder.Services.AddScoped<IAnalysisService, AnalysisService>();
builder.Services.AddScoped<IDocumentationService, DocumentationService>();
builder.Services.AddScoped<IBreakingChangeService, BreakingChangeService>();

// ── In-memory scan cache (24h TTL per repo URL) ────────────────────────────
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IScanCacheService, ScanCacheService>();

// ── SignalR (real-time analysis progress) ─────────────────────────────────
builder.Services.AddSignalR();
builder.Services.AddScoped<IAnalysisNotifier, AnalysisNotifier>();

// ── Auth services ──────────────────────────────────────────────────────────
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddHttpClient();

// ── Rate limiting (10 analysis requests/min per IP) ───────────────────────
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("analysis", limiterOptions =>
    {
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// ── CORS ────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var origins = builder.Configuration["ALLOWED_ORIGINS"];
        if (!string.IsNullOrWhiteSpace(origins))
        {
            policy
                .WithOrigins(origins.Split(',',
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    });
});

var app = builder.Build();

// ── Auto-migrate on startup (only when a real DB is configured) ────────────
if (!string.IsNullOrWhiteSpace(connectionString))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AnalysisHub>("/hubs/analysis");

app.Run();
