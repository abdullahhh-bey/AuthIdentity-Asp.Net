using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserAuthManagement.Data;
using UserAuthManagement.DTO;
using UserAuthManagement.Modals;

namespace UserAuthManagement.Services
{
    //WE MADE THE SERVICES FILE INSIDE THE SERVICES FOLDER SO THAT WE CAN MAKE OUR LOGIC HERE AND JUST USE IT IN CONTROLLERS
    // AND MAKE THE CONTROLLER LIGHTWEIGHT AND ALSO THIS IS A GOOD PRACTIE ACCORDING TO THE CLEAN ARCHITECTURE
    public class StudentService
    {
        private readonly UserAuthDbContext _context;
        private readonly IMapper _mapper;

        public StudentService(UserAuthDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //Make CRUD OPERATIONS here and DONT USE THE IACTIONRESULT  and just add simple return types of the functuons
        // because it is not a controller

        public async Task<List<StudentInfoDTO>> GetStudents()
        {
            var student = await _context.StudentDetails.ToListAsync();
            if (student == null || student.Count == 0)
            {
                return new List<StudentInfoDTO>();
            }

            var dto = _mapper.Map<List<StudentInfoDTO>>(student);
            return dto;
        }



        public async Task<StudentInfoDTO> GetStudentById(int id)
        {
            var student = await _context.StudentDetails.FindAsync(id);
            if (student == null)
            {
                return null;
            }

            var dto = _mapper.Map<StudentInfoDTO>(student);
            return dto;
        }


        public async Task<bool> AddStudent(CreateStudentDTO dto)
        {
            var checkEmail = await _context.StudentDetails.AnyAsync(s => s.Email == dto.Email);
            if (checkEmail)
            {
                return false;
            }

            var student = _mapper.Map<Student>(dto);
            _context.StudentDetails.Add(student);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> UpdateStudent(int id, UpdateStudentDTO dto)
        {
            var updatedStudent = await _context.StudentDetails.FindAsync(id);
            if (updatedStudent == null)
                return false;

            if (!string.IsNullOrEmpty(dto.Name))
            {
                updatedStudent.Name = dto.Name;
            }

            if (!string.IsNullOrEmpty(dto.Course))
            {
                updatedStudent.Course = dto.Course;
            }

            if (!string.IsNullOrEmpty(dto.Gender))
            {
                updatedStudent.Gender = dto.Gender;
            }

            if (!string.IsNullOrEmpty(dto.Email) && updatedStudent.Email != dto.Email)
            {
                var checkEmail = await _context.StudentDetails.AnyAsync(s => s.Email == dto.Email);
                if (checkEmail)
                {
                    return false;
                }
                updatedStudent.Email = dto.Email;
            }

            await _context.SaveChangesAsync();
            return true;
        }




        public async Task<bool> RemoveStudent(int id)
        {
            var student = await _context.StudentDetails.FindAsync(id);
            if (student == null)
                return false;

            _context.StudentDetails.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
