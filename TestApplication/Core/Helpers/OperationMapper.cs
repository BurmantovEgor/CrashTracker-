using AutoMapper;
using TestApplication.DataBase.Entities;
using TestApplication.DTO_s;

namespace CrashTracker.Core.Mapper_s
{
    public class OperationMapper : Profile
    {

        public OperationMapper()
        {
           
            CreateMap<OperationEntity, OperationDTO>().ReverseMap();

        }
    }
}
