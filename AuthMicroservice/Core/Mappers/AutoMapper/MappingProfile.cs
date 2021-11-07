using AuthMicroservice.Core.Fluent.Entities;
using AuthMicroservice.Core.Models.Dto.Person;
using AutoMapper;

namespace AuthMicroservice.Core.Mappers.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonCoreDto, Person>();
        }
    }
}
