namespace WebServer.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public required string Author { get; set; }
        public long TestId { get; set; }
        public required string TestComment { get; set; }
        public Test? Test { get; set; }
    }

    public class CommentRequest
    {
        public required string Author { get; set; }
        public long TestId { get; set; }
        public required string TestComment { get; set; }
        public Test? Test { get; set; }
    }
}
