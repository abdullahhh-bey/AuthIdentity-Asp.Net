using UserAuthManagement.DTO;

namespace UserAuthManagement.Repository
{
    public interface IAdvisorRepository
    {
        Task<List<AdvisorInfoDTO>> GetAdvisor();
        Task<List<AdvisorDetailsDTO>> GetAdvisorDetails();
        Task<AdvisorDetailsDTO> GetAdvisorDetailsById(int id);
        Task<AdvisorInfoDTO> GetAdvisorById(int id);
        Task<bool> AddAdvisor(CreateAdvisorDTO dto);
        Task<bool> UpdateAdvisor(int id, UpdateAdvisorDTO dto);
        Task<bool> DeleteAdvisor(string email);
    }
}
