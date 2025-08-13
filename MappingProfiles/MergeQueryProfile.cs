namespace ConnektaViz.API.MappingProfiles;

public class MergeQueryProfile : Profile
{
    public MergeQueryProfile()
    {
        CreateMap<MergeQuery, MergeQueryResponseDto>();
        CreateMap<MergeQueryDetail, MergeQueryDetailResponseDto>();

        CreateMap<MergeQueryRequestDto, MergeQuery>();
        CreateMap<MergeQueryDetailRequestDto, MergeQueryDetail>();
    }
}
