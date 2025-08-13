namespace ConnektaViz.API.MappingProfiles;

public class DashboardProfile : Profile
{
    public DashboardProfile()
    {

        // DTO to Entity mappings
        CreateMap<DashboardRequestDto, Dashboard>();
        CreateMap<DashboardGraphRequestDto, DashboardGraph>();


        CreateMap<Dashboard, DashboardResponseDto>();
        CreateMap<DashboardGraph, DashboardGraphResponseDto>();
    }
}
