
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using retrospectives_api.Controllers;
using retrospectives_api.DTOs;
using retrospectives_api.Models;
using retrospectives_api.Services;

namespace retrospectives_api_unit_tests;

public class RetrospectiveControllerTests 
{
    private readonly Mock<IRetrospectiveService> _mockRetrospectiveService;
    private readonly RetrospectiveController _sut;

    public RetrospectiveControllerTests()
    {
        _mockRetrospectiveService = new Mock<IRetrospectiveService>();
        
        var logger = new Mock<ILogger<RetrospectiveController>>();
        _sut = new RetrospectiveController(_mockRetrospectiveService.Object, logger.Object);
    }

    [Fact]
    public async Task CreateRetrospective_ReturnsOk_IfRetrospectiveIsCreated()
    {
        var retrospective = new RetrospectiveDTO()
        {
            Name = "Retrospective1",
            Date = DateTime.UtcNow,
            Participants = new List<string>() { "Participant1", "Participant2" },
            Summary = "Test retrospective 1",
        };

        _mockRetrospectiveService.Setup(service => service.CreateRetrospective(retrospective))
            .ReturnsAsync(retrospective);
        
        var result = await _sut.Create(retrospective);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var createdRetrospective = Assert.IsAssignableFrom<RetrospectiveDTO>(okResult.Value);
        Assert.Equal(retrospective.Name, createdRetrospective.Name);
    }

    [Fact]
    public async Task CreateRetrospective_Returns409_WhenAlreadyExists()
    {
        var retrospective = new RetrospectiveDTO()
        {
            Name = "Retrospective1",
            Date = DateTime.UtcNow,
            Participants = new List<string>() { "Participant1", "Participant2" },
            Summary = "Test retrospective 1",
        };

        _mockRetrospectiveService.Setup(service => service.CreateRetrospective(retrospective))
            .ThrowsAsync(new ArgumentException("Retrospective already exists"));
        
        var result = await _sut.Create(retrospective);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status409Conflict, objectResult.StatusCode);
        Assert.Equal("Retrospective already exists", objectResult.Value);
    }

    [Fact]
    public async Task CreateFeedback_ReturnsOk_WhenCreatedSuccessfully()
    {
        var feedbackItemDto = new FeedbackItemDTO()
        {
            Body = "Feedback1",
            Type = FeedbackType.Idea,
            CreatedBy = "Nick",
            RetrospectiveName = "Retrospective1"
        };

        var createdFeedbackItem = new FeedbackItemDTO()
        {
            Body = "Feedback1",
            Type = FeedbackType.Idea,
            CreatedBy = "Nick",
            RetrospectiveName = "Retrospective1"
        };

        _mockRetrospectiveService.Setup(service => service.CreateRetrospectiveFeedback(feedbackItemDto))
            .ReturnsAsync(createdFeedbackItem);
        
        var result = await _sut.CreateFeedback(feedbackItemDto);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<FeedbackItemDTO>(okResult.Value);
  
    }
    
    [Fact]
    public async Task CreateFeedback_Returns404_WhenRetrospectiveDoesntExist()
    {
        var feedbackItemDto = new FeedbackItemDTO()
        {
            Body = "Feedback1",
            Type = FeedbackType.Idea,
            CreatedBy = "Nick",
            RetrospectiveName = "DoesntExist"
        };

        _mockRetrospectiveService.Setup(service => service.CreateRetrospectiveFeedback(feedbackItemDto))
            .ThrowsAsync(new KeyNotFoundException("Retrospective not found"));
        
        var result = await _sut.CreateFeedback(feedbackItemDto);
        
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        Assert.Equal("Invalid retrospective", objectResult.Value);

    }

}
  

    
    