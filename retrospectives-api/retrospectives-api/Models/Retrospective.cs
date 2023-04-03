namespace retrospectives_api.Models;

public class Retrospective
{
    public string Name { get; set; }
    public string? Summary { get; set; }
    public DateTime Date { get; set; }
    public List<string> Participants { get; set; }
    public ICollection<FeedbackItem>? FeedbackItems { get; set; }
    
    
}