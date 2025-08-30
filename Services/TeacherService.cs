using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserAuthManagement.Data;
using UserAuthManagement.DTO;
using UserAuthManagement.Modals;

namespace UserAuthManagement.Services
{
    public class TeacherService
    {
        private readonly UserAuthDbContext _context;
        private readonly IMapper _mapper;

        public TeacherService(UserAuthDbContext context , IMapper mapper)
        {
            _context = context; 
            _mapper = mapper;
        }

        //Method/CRUD for Teacher
        public async Task<List<TeacherInfoDTO>> GetTeachers()
        {
            var teacher = await _context.TeacherDetails.ToListAsync();
            if (teacher.Count == 0)
                return new List<TeacherInfoDTO>();

            var dto = _mapper.Map<List<TeacherInfoDTO>>(teacher);
            return dto;
        }



        public async Task<List<TeacherDetailsDTO>> GetTeachersAll()
        {
            var teacher = await _context.TeacherDetails.ToListAsync();
            if (teacher.Count == 0)
                return new List<TeacherDetailsDTO>();

            var dto = _mapper.Map<List<TeacherDetailsDTO>>(teacher);
            return dto;
        }



        public async Task<TeacherInfoDTO> GetTeacherById(int id)
        {
            var teacher = await _context.TeacherDetails.FindAsync(id);
            if(teacher == null)
                return new TeacherInfoDTO();

            var dto = _mapper.Map<TeacherInfoDTO>(teacher);
            return dto;
        }



        public async Task<TeacherDetailsDTO> GetTeacherDetailsById(int id)
        {
            var teacher = await _context.TeacherDetails.FindAsync(id);
            if (teacher == null)
                return new TeacherDetailsDTO();

            var dto = _mapper.Map<TeacherDetailsDTO>(teacher);
            return dto;
        }


        public async Task<bool> AddTeacher(CreateTeacherDTO dto)
        {
            var teacher = await _context.TeacherDetails.AnyAsync(s => s.Email ==  dto.Email);
            if (teacher)
                return false;

            var s = _mapper.Map<Teacher>(dto);
            _context.TeacherDetails.Add(s);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> UpdateTeacher(int id, UpdateTeacher dto)
        {
            var UpdateTeacher = await _context.TeacherDetails.FindAsync(id);
            if (UpdateTeacher == null)
                return false;

            if (!string.IsNullOrEmpty(dto.Name))
            {
                UpdateTeacher.Name = dto.Name;
            }

            if (UpdateTeacher.Description == null)
            {
                UpdateTeacher.Description = new List<string>();
            }

            if (dto.Description.Count > 0)
            {
                UpdateTeacher.Description = dto.Description; 
            }

            if (dto.Salary.HasValue)
            {
                UpdateTeacher.Salary = dto.Salary.Value;
            }

            if (dto.Grade.HasValue)
            {
                UpdateTeacher.Grade = dto.Grade.Value;
            }

            if (!string.IsNullOrEmpty(dto.Gender))
            {
                UpdateTeacher.Gender = dto.Gender;
            }

            if (!string.IsNullOrEmpty(dto.Email) && UpdateTeacher.Email != dto.Email)
            {
                var checkEmail = await _context.TeacherDetails.AnyAsync(s => s.Email == dto.Email);
                if (checkEmail)
                {
                    return false;
                }
                UpdateTeacher.Email = dto.Email;
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> RemoveTeacher(string email)
        {
            var teacher = await _context.TeacherDetails.SingleOrDefaultAsync(s => s.Email == email);
            if (teacher == null)
                return false;

            _context.TeacherDetails.Remove(teacher);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
