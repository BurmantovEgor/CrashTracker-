using AutoMapper;
using TestApplication.Application.DTO_s;
using TestApplication.DataBase.Entities;
using TestApplication.DTO_s;

namespace CrashTracker.Core.Mapper_s
{
    public class CrashMapper : Profile
    {
        public CrashMapper()
        {
            CreateMap<CrashDTOCreate, CrashEntity>()
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid())) 
    .ForMember(dest => dest.CrashStatusId, opt => opt.MapFrom(src => src.Status)) 
    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
    .ForMember(dest => dest.CreatedById, opt => opt.MapFrom((src, dest, _, context) => context.Items["UserId"]));  


            CreateMap<CrashEntity, CrashDTO>()
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))  
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))  
          .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)) 
          .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.CrashStatusId.ToString()))  
          .ForMember(dest => dest.Operations, opt => opt.MapFrom(src => src.Operations))
          .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.CreatedBy.UserEmail)); 


        }


    }
}
