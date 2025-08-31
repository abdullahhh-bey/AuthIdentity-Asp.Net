using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserAuthManagement.Data;
using UserAuthManagement.DTO;

namespace UserAuthManagement.Repository
{
    public class AdvisorRepository : IAdvisorRepository
    {
        private readonly UserAuthDbContext _context;
        private readonly IMapper _mapper;

        public AdvisorRepository(UserAuthDbContext context, IMapper mapper) : base()
        {
             _context = context;
             _mapper = mapper;
        }


        public async Task<bool> AddAdvisor(CreateAdvisorDTO dto)
        {
            var a = await _context.AdvisorDetails.SingleOrDefaultAsync(a => a.Email == dto.Email);
            if(a == null)
                return false;

            await _context.AdvisorDetails.AddAsync(a);
            return true;
        }



        public async Task<bool> DeleteAdvisor(string email)
        {
            var a = await _context.AdvisorDetails.SingleOrDefaultAsync(a => a.Email == email);
            if (a == null)
                return false;

            _context.AdvisorDetails.Remove(a);
            return true;
        }



        public async Task<List<AdvisorInfoDTO>> GetAdvisor()
        {
            var  a = await _context.AdvisorDetails.ToListAsync();
            var dto = _mapper.Map<List<AdvisorInfoDTO>>(a);
            return dto;
        }



        public async Task<AdvisorInfoDTO> GetAdvisorById(int id)
        {
            var a = await _context.AdvisorDetails.FindAsync(id);
            var dto = _mapper.Map<AdvisorInfoDTO>(a);
            return dto;
        }



        public async Task<List<AdvisorDetailsDTO>> GetAdvisorDetails()
        {
            var a = await _context.AdvisorDetails.ToListAsync();
            var dto = _mapper.Map<List<AdvisorDetailsDTO>>(a);
            return dto;
        }



        public async Task<AdvisorDetailsDTO> GetAdvisorDetailsById(int id)
        {
            var a = await _context.AdvisorDetails.FindAsync(id);
            var dto = _mapper.Map<AdvisorDetailsDTO>(a);
            return dto;
        }



        public async Task<bool> UpdateAdvisor(int id, UpdateAdvisorDTO dto)
        {
            var updatedAdvisor = await _context.AdvisorDetails.FindAsync(id);
            if (updatedAdvisor == null)
                return false;


            if (!string.IsNullOrEmpty(dto.Name))
            {
                updatedAdvisor.Name = dto.Name;
            }

            if (dto.AdvisedCourses.Count > 0)
            {
                updatedAdvisor.AdvisedCourses = dto.AdvisedCourses;
            }

            if (dto.Salary.HasValue)
            {
                updatedAdvisor.Salary = dto.Salary.Value;
            }


            if (!string.IsNullOrEmpty(dto.Gender))
            {
                updatedAdvisor.Gender = dto.Gender;
            }


            if (!string.IsNullOrEmpty(dto.Email) && updatedAdvisor.Email != dto.Email)
            {
                var checkEmail = await _context.TeacherDetails.AnyAsync(s => s.Email == dto.Email);
                if (checkEmail)
                {
                    return false;
                }
                updatedAdvisor.Email = dto.Email;
            }

            
            return true;

        }


    }
}
