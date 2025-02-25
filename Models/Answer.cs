namespace WebServer.Models;

public class Answer
{
    public long Id { get; set; }
    public bool IsTrueAnswer { get; set; } = false;
    public string Body { get; set; } = "";
    public long QuestionId { get; set; }
}