using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.IdentityModel.Tokens;

namespace Futurum.WebApiEndpoint.Sample.Security;

public static class Login
{
    public record CommandDto(string Username, string Password, bool SetPermission, bool SetClaim, bool SetRole);

    public record Command(string Username, string Password, bool SetPermission, bool SetClaim, bool SetRole);

    public record Response(string JwtToken);

    public record ResponseDto(string JwtToken);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.Response<ResponseDto, Response>.Mapper<Mapper>
    {
        private readonly IConfiguration _configuration;

        public ApiEndpoint(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override Task<Result<Response>> ExecuteAsync(Command request, CancellationToken cancellationToken)
        {
            if (request.Username != "user1" || request.Password != "password1")
            {
                return Result.FailAsync<Response>($"Username or password entered is not recognised.");
            }

            var claims = new List<Claim>();

            if (request.SetPermission)
            {
                claims.Add(new Claim(AuthorizationConstants.ClaimType.Permissions, Authorization.Permission.Admin));
            }
            if (request.SetClaim)
            {
                claims.Add(new Claim(Authorization.ClaimType.Admin, Authorization.Claim.Admin));
            }
            if (request.SetRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, Authorization.Role.Admin));
            }

            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];
            var jwtKey = _configuration["Jwt:Key"];

            var claimsIdentity = new ClaimsIdentity(claims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(tokenHandler.CreateToken(descriptor));

            return Result.OkAsync(new Response(stringToken));
        }
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>, IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            new Command(dto.Username, dto.Password, dto.SetPermission, dto.SetClaim, dto.SetRole).ToResultOkAsync();

        public ResponseDto Map(Response domain) =>
            new (domain.JwtToken);
    }
}