using AutoMapper;
using SesTemplate.Application.Contracts.Dto;
using SesTemplate.Domain.Shared.Pagination;

namespace SesTemplate.Aplication.Services.AutoMapperProfiles;

public class AutoMapperProfileDto : Profile
{
    public AutoMapperProfileDto()
    {
        CreateMap(typeof(PagedResult<>), typeof(PagedResultDto<>))
            .ReverseMap()
            .PreserveReferences();

        CreateMap<PageInfo, PageInfoDto>()
            .ReverseMap();
        
    }
}
