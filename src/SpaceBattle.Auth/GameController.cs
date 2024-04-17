using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SpaceBattle.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;
    private readonly string key;
    public GameController(IMemoryCache memoryCache, IConfiguration configuration)
    {
        key = configuration.GetConnectionString("secret");
        _memoryCache = memoryCache;
    }

    [HttpPost()]
    public IActionResult Create(List<string> users)
    {
        var gameId = Guid.NewGuid();
        _memoryCache.Set(gameId, JsonSerializer.Serialize(users));
        return Ok(gameId);
    }

    [HttpGet("Verify")]
    [Authorize]
    public IActionResult Verify()
    {
        return Ok(HttpContext.User.Identity.Name);
    }
    [HttpGet()]
    public IActionResult GetToken(Guid gameId, string user)
    {
        var users = _memoryCache.Get(gameId);
        if (users is null)
        {
            return NotFound();
        }
        if (JsonSerializer.Deserialize<List<string>>((string)users).Any(x => x == user) is false)
        {
            return Unauthorized();
        }

        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(claims: [new(ClaimsIdentity.DefaultNameClaimType, user), new Claim("gameId", gameId.ToString())],
                                       notBefore: now,
                                       expires: now.Add(TimeSpan.FromMinutes(30)),
                                       signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var response = new
        {
            access_token = encodedJwt,
            username = user
        };

        return Ok(response);
    }
}