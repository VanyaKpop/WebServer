namespace WebServer.Models
{
    public class User
    {
		public long Id { get; set; }
		public required string Username { get; set; }
		public required string Password { get; set; }
		public required string Role { get; set; }
    }
	public class UserRequest
    {
		public required string Username { get; set; }
		public required string Password { get; set; }
    }
}


