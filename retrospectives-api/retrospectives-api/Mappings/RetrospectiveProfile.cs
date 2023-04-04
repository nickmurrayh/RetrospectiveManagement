using AutoMapper;
using retrospectives_api.DTOs;
using retrospectives_api.Models;

namespace retrospectives_api.Mappings;

public class RetrospectiveProfile : Profile
{
    public RetrospectiveProfile()
    {
        CreateMap<Retrospective, RetrospectiveDTO>();
        CreateMap<RetrospectiveDTO, Retrospective>();
    }
}