using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebServer.Models;

public class AuthOptions
{
    public const string ISSUER = "WebServer";
    public const string AUDIENCE = "AuthClient";
    public const string Key = "DSewqfpoRFWREdpmWEeFdqGerWRDsgGqwvfaRMYva";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
}
 