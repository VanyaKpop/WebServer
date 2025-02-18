using Microsoft.AspNetCore.Mvc;
using WebServer.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebServer.Models;
using Microsoft.AspNetCore.Cors;

namespace WebServer.Controllers;

public record GetForeignTokenResponse(string Token);

[ApiController]
[Route("Auth")]
public class AuthorizationController : Controller
{
    private ILogger<AuthorizationController> _logger;

    public AuthorizationController(ILogger<AuthorizationController> logger){
        _logger = logger;
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register([FromServices] IDBService service, [FromBody] UserRequest user)
    {
        if (await service.IsUserExist(user.name))
            return BadRequest("User with the same name alredy exist");

        if (user.name == null || user.password == null)
            return BadRequest();

        await Task.Run(() => service.AddUser(user.name, user.password));

        return Created();
    }

    private ClaimsIdentity GetIdentity([FromServices] IDBService service, string username, string password)
    {
        User person = service.GetUser(username, password);
        if (person != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, person.name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, person.role)
            };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        // если пользователя не найдено
        return null;
    }

    [HttpGet("Login/{username}/{password}")]
    public async Task<ActionResult> Login([FromServices] IDBService service, string username, string password)
    {
        var identity = GetIdentity(service, username, password);

        if (identity == null)
        {
            return BadRequest(new { errorText = "Invalid username or password." });
        }

        if (await service.IsUserExist(username, password) == false)
        return NotFound();

        if (username == null | password == null)
            return BadRequest("username or password is null");


        var jwt = new JwtSecurityToken (
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return Json(new GetForeignTokenResponse(token), Extensions.JsonOptions);
    }

}
