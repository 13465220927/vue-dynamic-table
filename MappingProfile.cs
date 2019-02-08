using AutoMapper;
using Logistyx.Bios.WebApp.Models;
using Logistyx.Bios.WebApp.Entities;

namespace Logistyx.Bios.WebApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Resource, ResourceForManipulationDto>();
            CreateMap<ResourceForManipulationDto, Resource>();

            CreateMap<Resource, ResourceForCreationDto>();
            CreateMap<ResourceForCreationDto, Resource>();
        }
    }
}
