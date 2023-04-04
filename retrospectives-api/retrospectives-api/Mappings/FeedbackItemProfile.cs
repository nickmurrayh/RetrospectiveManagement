using AutoMapper;
using retrospectives_api.DTOs;
using retrospectives_api.Models;

namespace retrospectives_api.Mappings;

public class FeedbackItemProfile : Profile
{
    public FeedbackItemProfile()
    {
        CreateMap<FeedbackItem, FeedbackItemDTO>();
        CreateMap<FeedbackItemDTO, FeedbackItem>();
    }
    
}