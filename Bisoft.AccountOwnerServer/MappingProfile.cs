using AutoMapper;
using Entities.DataTransferObjects;
using Entities.models;

namespace Bisoft.AccountOwnerServer
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Owner, OwnerDto>();
        }
    }
}
