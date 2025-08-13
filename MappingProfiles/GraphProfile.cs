namespace ConnektaViz.API.MapperProfiles;

public class GraphProfile : Profile
{
    public GraphProfile()
    {
        CreateMap<GraphType, GraphTypeResponseDto>().ReverseMap();
        CreateMap<Graph, GraphResponseDto>().ReverseMap();
        CreateMap<GraphRequestDto, Graph>().ReverseMap();

        CreateMap<GraphColumn, GraphColumnResponseDto>().ReverseMap();
        CreateMap<GraphColumnRequestDto, GraphColumn>().ReverseMap();


        CreateMap<SelectedTableColumnRequestDto, SelectedTableColumn>().ReverseMap();
        CreateMap<SelectedTableColumnResponseDto, SelectedTableColumn>().ReverseMap();

        // Add mappings for GraphStyling
        CreateMap<GraphStyling, GraphStylingResponseDto>().ReverseMap();
        CreateMap<GraphStylingRequestDto, GraphStyling>().ReverseMap();

        CreateMap<GraphTableFilterRequestDto, GraphTableFilter>()
            .ForMember(x => x.Field,x => x.MapFrom(m => m.Name))
            .ForMember(x => x.Operator,x => x.MapFrom(m => m.Operation))
            .ForMember(x => x.DisplayOperator, x => x.MapFrom(m => m.OperationKey))
            .ReverseMap();
        
        CreateMap<GraphTableFilterResponseDto, GraphTableFilter>()
            .ForMember(x => x.Field,x => x.MapFrom(m => m.Name))
            .ForMember(x => x.Operator,x => x.MapFrom(m => m.Operation))
            .ForMember(x => x.DisplayOperator, x => x.MapFrom(m => m.OperationKey))
            .ReverseMap();
    }
}
