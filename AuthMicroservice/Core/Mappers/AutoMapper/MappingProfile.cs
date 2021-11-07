using AuthMicroservice.Core.Fluent.Entities;
using AuthMicroservice.Core.Models.Dto.Enterprise;
using AuthMicroservice.Core.Models.Dto.Person;
using AutoMapper;

namespace AuthMicroservice.Core.Mappers.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonCoreDto, Person>()
                 .ForMember(dest => dest.UserDomain, opt => opt.Ignore());

            CreateMap<EnterpriseCoreDto, Enterprise>()
                .ForMember(dest => dest.EnterprisesToUsersDomains, opt => opt.Ignore());
        }
    }
}
