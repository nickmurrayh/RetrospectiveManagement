namespace retrospectives_api.Models;

public class FeedbackItem
{
    public int Id { get; set; }
    public string CreatedBy { get; set; }
    public string? Body { get; set; }
    public FeedbackType Type { get; set; }
    public virtual Retrospective Retrospective { get; set; }
    public string? RetrospectiveName { get; set; }
}

public enum FeedbackType
{
    Positive,
    Negative,
    Idea,
    Praise
}


