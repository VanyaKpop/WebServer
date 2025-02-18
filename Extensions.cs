using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace WebServer;

public static class Extensions
{
    public readonly static JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}