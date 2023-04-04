using retrospectives_api.Models;

namespace retrospectives_api.DTOs;

public class FeedbackItemDTO
{
    public string CreatedBy { get; set; }
    public string? Body { get; set; }
    public FeedbackType Type { get; set; }
    public string RetrospectiveName { get; set; }
}