using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CineMatch.Application.Common.Exceptions.Auth;
using CineMatch.Application.Common.Interfaces;
using CineMatch.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CineMatch.Application.Features.Token;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor,
        IApplicationDbContext context)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public void SetRefreshToken(Domain.Entities.User user, RefreshToken refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = refreshToken.Expired
        };

        if (_httpContextAccessor.HttpContext != null)
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        user.RefreshToken = refreshToken;
    }

    public async Task<string> RefreshToken()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var user = await _context.Users.Include(user => user.RefreshToken)
                .FirstOrDefaultAsync(user => user.Username == _httpContextAccessor.HttpContext.User.Identity.Name);

            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            if (!user.RefreshToken.Token.Equals(refreshToken)) throw new TokenException("Invalid refresh token");

            if (user.RefreshToken.Expired < DateTime.UtcNow) throw new TokenException("Token expired");

            SetRefreshToken(user, GenerateRefreshToken());
            return CreateToken(user.Username);
        }

        throw new TokenException("Error refreshing token");
    }

    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expired = DateTime.UtcNow.AddDays(7)
        };

        return refreshToken;
    }

    public string CreateToken(string username)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username)
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            null,
            null,
            claims,
            null,
            DateTime.UtcNow.AddDays(1),
            credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}