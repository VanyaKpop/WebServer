
namespace WebServer.Models;

public class Question
{
    public long Id { get; set; }
    public string Type { get; set; }
    public bool IsRequired { get; set; }
    public string Body { get; set; }
    public long TestId { get; set; }
    public List<Answer> Answers { get; set; } = new();
}
