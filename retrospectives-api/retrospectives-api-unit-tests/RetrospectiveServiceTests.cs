using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using retrospectives_api.Data;
using retrospectives_api.DTOs;
using retrospectives_api.Models;
using retrospectives_api.Services;

namespace retrospectives_api_unit_tests;

public class RetrospectiveServiceTests : IDisposable
{

    private readonly AppDbContext _appDbContext;

    public RetrospectiveServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _appDbContext = new AppDbContext(options);
    }
    
    [Fact]
    public async Task GetAll_ReturnsAllRetrospectives()
    {
        var retrospectives = new List<Retrospective>()
       {
           new()
           {
               Name = "Retrospective1",
               Date = DateTime.UtcNow,
               Participants = new List<string>() { "Participant1", "Participant2" },
               Summary = "Test retrospective 1",
           },
           new()
           {
               Name = "Retrospective2",
               Date = DateTime.UtcNow,
               Participants = new List<string>() { "Participant1", "Participant2" },
               Summary = "Test retrospective 2",
           }
       }.AsQueryable();
        
        await _appDbContext.Retrospectives.AddRangeAsync(retrospectives);
        await _appDbContext.SaveChangesAsync();
        
        var mockLogger = new Mock<ILogger<RetrospectiveService>>();
        var mockMapper = new Mock<IMapper>();
        
        mockMapper.Setup(m => m.Map<IEnumerable<RetrospectiveDTO>>(It.IsAny<IEnumerable<Retrospective>>())).Returns(new List<RetrospectiveDTO>()
        {
            new()
            {
                Name = "Retrospective4",
                Date = DateTime.UtcNow,
                Participants = new List<string>() { "Participant1", "Participant2" },
                Summary = "Test retrospective 1", 
            },
            new()
            {
                Name = "Retrospective5",
                Date = DateTime.UtcNow,
                Participants = new List<string>() { "Participant1", "Participant2" },
                Summary = "Test retrospective 2",
            }
        });
        
        var sut = new RetrospectiveService(_appDbContext, mockLogger.Object, mockMapper.Object);
        
        var allRetrospectives = await sut.GetRetrospectives();
        
        Assert.Equal(2, allRetrospectives.Count());
        
    }
    
    
    [Fact]
    public async Task GetRetrospectivesByDate_ReturnsCorrectRetrospectives()
    {
        var retrospectives = new List<Retrospective>()
        {
            new()
            {
                Name = "Retrospective1",
                Date = new DateTime(2023,04,04),
                Participants = new List<string>() { "Participant1", "Participant2" },
                Summary = "Test retrospective 1",
            },
            new()
            {
                Name = "Retrospective2",
                Date = new DateTime(2023,04,01),
                Participants = new List<string>() { "Participant1", "Participant2" },
                Summary = "Test retrospective 2",
            },
            new()
            {
                Name = "Retrospective3",
                Date = new DateTime(2023,04,01),
                Participants = new List<string>() { "Participant1", "Participant2" },
                Summary = "Test retrospective 3",
            }
        }.AsQueryable();

        
        _appDbContext.Retrospectives.AddRange(retrospectives);
        _appDbContext.SaveChanges();
        
        var mockLogger = new Mock<ILogger<RetrospectiveService>>();
        var mockMapper = new Mock<IMapper>();

        mockMapper.Setup(m => m.Map<IEnumerable<RetrospectiveDTO>>(It.IsAny<IEnumerable<Retrospective>>())).Returns(new List<RetrospectiveDTO>()
        {
            new()
            {
                Name = "Retrospective2",
                Date = new DateTime(2023,04,04),
                Participants = new List<string>() { "Participant1", "Participant2" },
                Summary = "Test retrospective 2",
            },
            new()
            {
                Name = "Retrospective3",
                Date = new DateTime(2023,04,04),
                Participants = new List<string>() { "Participant1", "Participant2" },
                Summary = "Test retrospective 3",
            },
        });
        
        var sut = new RetrospectiveService(_appDbContext, mockLogger.Object, mockMapper.Object);
        
        
        var allRetrospectives = await sut.GetRetrospectivesByDate(new DateTime(2023,04,01));
        
        Assert.Equal(2,allRetrospectives.Count());
        
    }
    
    [Fact]
    public async Task CreateNewRetrospective_OnSuccess_ReturnsRetrospective()
    {
        var retrospective = new RetrospectiveDTO()
        {
            Name = "Retrospective1",
            Date = new DateTime(2023,04,04),
            Participants = new List<string>() { "Participant1", "Participant2" },
            Summary = "Test retrospective 1",
        };
        
        var mockLogger = new Mock<ILogger<RetrospectiveService>>();
        var mockMapper = new Mock<IMapper>();
        
        mockMapper.Setup(m => m.Map<Retrospective>(It.IsAny<RetrospectiveDTO>())).Returns(new Retrospective()
        {
            Name = "Retrospective1",
            Date = new DateTime(2023,04,04),
            Participants = new List<string>() { "Participant1", "Participant2" },
            Summary = "Test retrospective 1",
        });
        
        var sut = new RetrospectiveService(_appDbContext, mockLogger.Object, mockMapper.Object);
        
        var createdRetrospective = await sut.CreateRetrospective(retrospective);
        
        Assert.Equal(retrospective.Name, createdRetrospective.Name);
    }

    [Fact]
    public async Task CreateNewFeedback_IfRetrospectiveExists_ReturnsFeedbackItem()
    {
        var retrospective = new Retrospective()
        {
            Name = "Retrospective1",
            Date = new DateTime(2023,04,04),
            Participants = new List<string>() { "Participant1", "Participant2" },
            Summary = "Test retrospective 1",
        };

        var feedbackItem = new FeedbackItemDTO()
        {
            Body = "Feedback1",
            Type = FeedbackType.Idea,
            CreatedBy = "Nick",
            RetrospectiveName = "Retrospective1"
        };
        
        _appDbContext.Retrospectives.Add(retrospective);
        _appDbContext.SaveChanges();
        
        var mockLogger = new Mock<ILogger<RetrospectiveService>>();
        var mockMapper = new Mock<IMapper>();
        
        mockMapper.Setup(m => m.Map<FeedbackItem>(It.IsAny<FeedbackItemDTO>())).Returns(new FeedbackItem()
        {
            Body = "Feedback1",
            Type = FeedbackType.Idea,
            CreatedBy = "Nick",
            Retrospective = retrospective,
            RetrospectiveName = "Retrospective1"
        });
        
        var sut = new RetrospectiveService(_appDbContext, mockLogger.Object, mockMapper.Object);

        var createdFeedbackItem = await sut.CreateRetrospectiveFeedback(feedbackItem);
        
        Assert.NotNull(createdFeedbackItem);
    
        
       
    }
    
    [Fact]
    public async Task CreateNewFeedback_IfRetrospectiveDoesNotExist_ThrowsError()
    {

        var feedbackItem = new FeedbackItemDTO()
        {
            Body = "Feedback1",
            Type = FeedbackType.Idea,
            CreatedBy = "Nick",
            RetrospectiveName = "Retrospective3"
        };
        
        var mockLogger = new Mock<ILogger<RetrospectiveService>>();
        var mockMapper = new Mock<IMapper>();

        mockMapper.Setup(m => m.Map<Retrospective>(It.IsAny<RetrospectiveDTO>())).Returns(new Retrospective());
        
        var sut = new RetrospectiveService(_appDbContext, mockLogger.Object, mockMapper.Object);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () => await sut.CreateRetrospectiveFeedback(feedbackItem));
        
        Assert.Equal("Retrospective not found", exception.Message);
    }
    
    public void Dispose()
    {
        _appDbContext.Database.EnsureDeleted();
        _appDbContext.Dispose();
    }
}