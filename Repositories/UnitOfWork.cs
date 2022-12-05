using VoteMovie.Entity;
using VoteMovie.Entity.Abstract;
using VoteMovie.Repositories.Abstract;

namespace VoteMovie.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MoviewAppContext baseContext;
        private Dictionary<string, object> repositoriesCRUD;
        private Dictionary<string, object> _repositoriesProc;

        public UnitOfWork(MoviewAppContext BaseContext)
        {
            baseContext = BaseContext;
            repositoriesCRUD = new Dictionary<string, object>();
        }

        public IBaseRepository<T> RepositoryCRUD<T>() where T : class, IEntityBase, new()
        {
            if (repositoriesCRUD == null)
            {
                repositoriesCRUD = new Dictionary<string, object>();
            }
            var type = typeof(T).Name;
            if (!repositoriesCRUD.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), baseContext);
                repositoriesCRUD.Add(type, repositoryInstance);
            }
            return (BaseRepository<T>)repositoriesCRUD[type];
        }

        public void Commit()
        {
            baseContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await baseContext.SaveChangesAsync();
        }
    }
}
