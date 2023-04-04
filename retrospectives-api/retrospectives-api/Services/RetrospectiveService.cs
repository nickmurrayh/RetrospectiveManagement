using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using retrospectives_api.Data;
using retrospectives_api.DTOs;
using retrospectives_api.Models;
using Serilog;

namespace retrospectives_api.Services;

public class RetrospectiveService : IRetrospectiveService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<RetrospectiveService> _logger;
    private readonly IMapper _mapper;

    public RetrospectiveService(AppDbContext dbContext, ILogger<RetrospectiveService> logger, IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<IEnumerable<RetrospectiveDTO>?> GetRetrospectives()
    {
        _logger.LogInformation($"Retrieving all retrospectives");
        var retrospectives = await _dbContext.Retrospectives
            .OrderByDescending(r => r.Date)
            .Include(r => r.FeedbackItems)
            .ToListAsync();
        
        var retrospectiveDTOs = _mapper.Map<IEnumerable<RetrospectiveDTO>>(retrospectives);
        return retrospectiveDTOs;
        
    }

    public async Task<IEnumerable<RetrospectiveDTO>?> GetRetrospectivesByDate(DateTime date)
    {
        var retrospectives = await _dbContext.Retrospectives
            .Where(r => r.Date == date.Date)
            .OrderByDescending(r => r.Date)
            .Include(r => r.FeedbackItems)
            .ToListAsync();
        var retrospectiveDTOs = _mapper.Map<IEnumerable<RetrospectiveDTO>>(retrospectives);
        return retrospectiveDTOs;
    }


    public async Task<RetrospectiveDTO?> CreateRetrospective(RetrospectiveDTO retrospective)
    {
        _logger.LogInformation($"Creating retrospective {retrospective.Name}");
        var retrospectiveEntity = _mapper.Map<Retrospective>(retrospective);
        await _dbContext.Retrospectives.AddAsync(retrospectiveEntity);
        await _dbContext.SaveChangesAsync();
        return retrospective;
    }

    public async Task<FeedbackItemDTO?> CreateRetrospectiveFeedback(FeedbackItemDTO feedbackItem)
    {
        var retrospective = await _dbContext.Retrospectives.FindAsync(feedbackItem.RetrospectiveName);
        if (retrospective == null)
        {
            throw new KeyNotFoundException("Retrospective not found");
        }

        retrospective.FeedbackItems ??= new List<FeedbackItem>();

        var feedbackItemEntity = _mapper.Map<FeedbackItem>(feedbackItem);
        feedbackItemEntity.Retrospective = retrospective;
        
        retrospective.FeedbackItems.Add(feedbackItemEntity);
        
        await _dbContext.SaveChangesAsync();
        return feedbackItem;
    }
}