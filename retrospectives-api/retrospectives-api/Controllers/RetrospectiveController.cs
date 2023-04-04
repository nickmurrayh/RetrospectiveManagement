using System.Linq.Expressions;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using retrospectives_api.DTOs;
using retrospectives_api.Models;
using retrospectives_api.Services;
using Serilog;
using ILogger = Castle.Core.Logging.ILogger;

namespace retrospectives_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RetrospectiveController : ControllerBase
{
    private readonly IRetrospectiveService _retrospectiveService;
    private readonly ILogger<RetrospectiveController> _logger;

    public RetrospectiveController(IRetrospectiveService retrospectiveService, ILogger<RetrospectiveController> logger)
    {
        _retrospectiveService = retrospectiveService;
        _logger = logger;
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.LogInformation($"Fetching all retrospectives");
            var retrospectives = await _retrospectiveService.GetRetrospectives();
            return Ok(retrospectives);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred when fetching all retrospectives: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByDate([FromQuery] DateTime date)
    {
        try
        {
            _logger.LogInformation($"Fetching retrospectives by date: {date}");
            var retrospectives = await _retrospectiveService.GetRetrospectivesByDate(date);
            return Ok(retrospectives);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred when fetching retrospectives by date: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RetrospectiveDTO retrospective)
    {
        try
        {
            _logger.LogInformation($"Creating new retrospective: {retrospective.Name}");
            var createdRetrospective = await _retrospectiveService.CreateRetrospective(retrospective);
            return Ok(createdRetrospective);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Unable to create retrospective as it already exists");
            return StatusCode(StatusCodes.Status409Conflict, "Retrospective already exists");
        }
    }

    [HttpPost("feedback")]
    public async Task<IActionResult> CreateFeedback([FromBody] FeedbackItemDTO feedbackItem)
    {
        try
        {
            _logger.LogInformation($"Creating new feedback");
            var createdFeedbackItem = await _retrospectiveService.CreateRetrospectiveFeedback(feedbackItem);
            return Ok(createdFeedbackItem);
        }
        catch (KeyNotFoundException)
        {
            _logger.LogError($"Error creating feedback, unable to find retrospective");
            return StatusCode(StatusCodes.Status404NotFound, "Invalid retrospective");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating feedback: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    
    
}