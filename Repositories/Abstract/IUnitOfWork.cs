using VoteMovie.Entity.Abstract;

namespace VoteMovie.Repositories.Abstract
{
    public interface IUnitOfWork
    {
        IBaseRepository<T> RepositoryCRUD<T>() where T : class, IEntityBase, new();
        void Commit();
        Task<int> CommitAsync();
    }
}
