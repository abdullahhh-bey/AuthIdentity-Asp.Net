
using UserAuthManagement.Data;

namespace UserAuthManagement.Repository
{
    public class UnitOfWork : IUnitofWork
    {
        public UserAuthDbContext _context;
        public IAdvisorRepository AdvisorRepository { get; private set; }

        public UnitOfWork(UserAuthDbContext Context , IAdvisorRepository advisorRepository)
        {
                _context = Context;
                AdvisorRepository = advisorRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
             _context.Dispose();
        }
    }
}
