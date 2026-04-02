using Alarm112.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Alarm112.Api.Endpoints;

public static class AuthEndpoints
{
    private static readonly string[] ValidRoles =
        ["CallOperator", "Dispatcher", "OperationsCoordinator", "CrisisOfficer"];

    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/auth/dev-token",
            (DevTokenRequest request, IConfiguration configuration) =>
            {
                var enabled = configuration.GetValue<bool>("Security:EnableDevTokenEndpoint");
                if (!enabled)
                    return Results.NotFound();

                var role = request.Role ?? "CallOperator";
                if (!ValidRoles.Contains(role))
                    return Results.BadRequest(new
                    {
                        error = $"Invalid role '{role}'. Must be one of: {string.Join(", ", ValidRoles)}"
                    });

                var issuer   = configuration["Security:Jwt:Issuer"]    ?? "Alarm112.Api";
                var audience = configuration["Security:Jwt:Audience"]  ?? "Alarm112.Client";
                var signingKey = configuration["Security:Jwt:SigningKey"];

                if (string.IsNullOrWhiteSpace(signingKey) || signingKey.Length < 32)
                    return Results.Problem("JWT signing key is not configured.",
                        statusCode: StatusCodes.Status500InternalServerError);

                var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Sub, request.Subject ?? "dev-user"),
                    new(ClaimTypes.Name, request.Subject ?? "dev-user"),
                    new(ClaimTypes.Role, role)
                };

                var credentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer, audience, claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: credentials);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return Results.Ok(new { accessToken = jwt, tokenType = "Bearer", expiresIn = 3600, role });
            }).AllowAnonymous();

        return app;
    }
}
