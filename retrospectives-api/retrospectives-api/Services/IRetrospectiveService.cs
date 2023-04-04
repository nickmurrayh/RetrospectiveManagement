using System.Linq.Expressions;
using retrospectives_api.DTOs;
using retrospectives_api.Models;

namespace retrospectives_api.Services;

public interface IRetrospectiveService
{
    public Task<IEnumerable<RetrospectiveDTO>?> GetRetrospectives();
    public Task<IEnumerable<RetrospectiveDTO>?> GetRetrospectivesByDate(DateTime date);
    public Task<RetrospectiveDTO> CreateRetrospective(RetrospectiveDTO retrospective);
    public Task<FeedbackItemDTO> CreateRetrospectiveFeedback(FeedbackItemDTO feedbackItem);
}