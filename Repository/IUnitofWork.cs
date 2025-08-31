namespace UserAuthManagement.Repository
{
    public interface IUnitofWork : IDisposable
    {
        IAdvisorRepository AdvisorRepository { get; }
        Task<int> CompleteAsync();
    }
}
