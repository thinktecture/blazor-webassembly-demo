using AutoMapper;

namespace BlazorConfTool.Server
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Model.Conference, Shared.DTO.ConferenceOverview>();
            CreateMap<Shared.DTO.ConferenceOverview, Model.Conference>();
            CreateMap<Model.Conference, Shared.DTO.ConferenceDetails>();
            CreateMap<Shared.DTO.ConferenceDetails, Model.Conference>();
        }
    }
}
