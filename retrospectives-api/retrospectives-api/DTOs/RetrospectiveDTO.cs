namespace retrospectives_api.DTOs;

public class RetrospectiveDTO
{
    public string Name { get; set; }
    public string? Summary { get; set; }
    public DateTime? Date { get; set; }
    public List<string> Participants { get; set; }
    public ICollection<FeedbackItemDTO>? FeedbackItems { get; set; }
}