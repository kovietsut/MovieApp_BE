using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VoteMovie.Entity.Abstract;

namespace VoteMovie.Repositories.Abstract
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntityBase
    {
        DbSet<TEntity> Set();
        int Count();
        Task<int> CountAsync();
        bool Any(Expression<Func<TEntity, bool>> predicate);
        bool Exist(int id);
        List<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        TEntity GetByID(int id);
        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate);
        TEntity GetSingleWithNoIsEnable(Expression<Func<TEntity, bool>> predicate);
        void Delete(TEntity entityToDelete);
        void Delete(int id);
        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        void InsertAndUpdate(TEntity entity);
    }
}
