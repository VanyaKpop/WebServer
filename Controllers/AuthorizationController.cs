using Microsoft.AspNetCore.Mvc;
using WebServer.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebServer.Models;
using Microsoft.AspNetCore.Cors;
using System.Text.RegularExpressions;

namespace WebServer.Controllers;

public record GetForeignTokenResponse(string Token);

[ApiController]
[Route("Auth")]
public partial class AuthorizationController : Controller
{
    [GeneratedRegex("^[a-zA-Z0-9_]+$")]
    private static partial Regex IsUsernameValid();
    private ILogger<AuthorizationController> _logger;

    public AuthorizationController(ILogger<AuthorizationController> logger){
        _logger = logger;
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register([FromServices] IDBService service, [FromBody] UserRequest user)
    {
        if (await service.IsUserExist(user.Username))
            return BadRequest("User with the same name alredy exist");

        if (!IsUsernameValid().IsMatch(user.Username) || user.Username.Contains("__") || user.Username.StartsWith('_') || user.Username.Length > 48 || user.Username.Length < 3)
        {
            return ValidationProblem("Username can only contain alphanumeric characters and 3-48 characters");
        }
        if (user.Username == null || user.Password == null)
            return BadRequest();

        await Task.Run(() => service.AddUser(user.Username, user.Password));

        return Created();
    }

    private ClaimsIdentity? GetIdentity([FromServices] IDBService service, string username, string password)
    {
        User? person = service.GetUser(username, password);
        if (person != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, person.Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
            };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        // если пользователя не найдено
        return null;
    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromServices] IDBService service, [FromBody] UserRequest userRequest)
    {
        var identity = GetIdentity(service, userRequest.Username, userRequest.Password);

        if (identity == null)
        {
            return BadRequest(new { errorText = "Invalid username or password." });
        }

        if (await service.IsUserExist(userRequest.Username) == false)
            return NotFound();

        if (userRequest.Username == null | userRequest.Username == null)
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
