using ConnektaViz.API.Entities.Connekta;

namespace ConnektaViz.API.MappingProfiles;

public class UserProfile:Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.UserId, src => src.MapFrom(m => m.ID));
    }
}
