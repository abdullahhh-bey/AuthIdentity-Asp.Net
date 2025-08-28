using AutoMapper;
using UserAuthManagement.DTO;
using UserAuthManagement.Modals;

namespace UserAuthManagement.Mapping
{
    public class MappingUsers : Profile
    {
        public MappingUsers()
        {

            //Create the Mappings
            CreateMap<RegisterDTO, User>(); //DTO -> ENITITY
            CreateMap<LoginDTO, User>(); //DTO -> ENTITY
            CreateMap<User, UsersNameDTO>(); //ENTITY -> DTO

        }
    }
}
