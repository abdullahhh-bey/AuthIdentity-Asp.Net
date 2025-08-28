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
            CreateMap<RegisterDTO, User>()
            // The default mapping will handle FullName
            // .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            // Explicitly map the Email property from the DTO to the UserName property of the User entity.
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<LoginDTO, User>(); //DTO -> ENTITY
            CreateMap<User, UsersNameDTO>(); //ENTITY -> DTO
            CreateMap<Student, StudentInfoDTO>(); //ENTITY -> DTO
            CreateMap<CreateStudentDTO, Student>(); //DTO -> ENTITY

        }
    }
}
