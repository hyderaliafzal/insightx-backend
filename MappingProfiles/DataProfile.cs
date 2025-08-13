namespace ConnektaViz.API.MappingProfiles;

public class DataProfile : Profile
{
    public DataProfile()
    {
        CreateMap<TableVW, TableResponseDto>().ReverseMap();
        CreateMap<ColumnSP, ColumnResponseDto>().ReverseMap();
    }
}
