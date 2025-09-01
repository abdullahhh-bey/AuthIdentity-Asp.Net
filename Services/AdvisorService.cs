using UserAuthManagement.DTO;
using UserAuthManagement.Repository;

namespace UserAuthManagement.Services
{
    public class AdvisorService
    {
        private readonly IUnitofWork _unitofwork;

        public AdvisorService(IUnitofWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task<List<AdvisorInfoDTO>> GetAllAdvisor()
        {
            var dto = await _unitofwork.AdvisorRepository.GetAdvisor();
            if(dto.Count == 0)
                return new List<AdvisorInfoDTO>();

            return dto;
        }


        public async Task<List<AdvisorDetailsDTO>> GetAllAdvisorDetails()
        {
            var dto = await _unitofwork.AdvisorRepository.GetAdvisorDetails();
            if (dto.Count == 0)
                return new List<AdvisorDetailsDTO>();

            return dto;
        }


        public async Task<AdvisorInfoDTO> GetAdvisorById(int id)
        {
            var dto = await _unitofwork.AdvisorRepository.GetAdvisorById(id);
            if (dto == null)
                return new AdvisorInfoDTO(); 

            return dto;
        }


        public async Task<AdvisorDetailsDTO> GetAdvisorDetailsById(int id)
        {
            var dto = await _unitofwork.AdvisorRepository.GetAdvisorDetailsById(id);
            if (dto == null)
                return new AdvisorDetailsDTO();

            return dto;
        }



        public async Task<bool> CreateAdvisor(CreateAdvisorDTO dto)
        {
            var check = await _unitofwork.AdvisorRepository.AddAdvisor(dto);
            if (!check)
                return false;

            await _unitofwork.CompleteAsync();
            return true;
        }



        public async Task<bool> UpdateAdvisor(int id,  UpdateAdvisorDTO dto)
        {
            var check = await _unitofwork.AdvisorRepository.UpdateAdvisor(id, dto);
            if (!check)
                return false;

            await _unitofwork.CompleteAsync();
            return true;
        }



        public async Task<bool> RemoveAdvisor(string email)
        {
            var check = await _unitofwork.AdvisorRepository.DeleteAdvisor(email);
            if (!check)  
                return false;

            await _unitofwork.CompleteAsync();
            return true;
        }


    }
}
