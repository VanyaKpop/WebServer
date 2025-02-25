namespace WebServer.Models
{
    public class Test
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public List<Question> Questions { get; set; } = new();
    }

    public class TestRequest
    {
        public required string Name { get; set; }
        public required string Author { get; set; }
        public List<Question> Questions { get; set; } = new();
        public List<Answer> Answers { get; set; } = new();
    }
}
