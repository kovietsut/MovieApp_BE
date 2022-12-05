using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VoteMovie.Entity;
using VoteMovie.Entity.Abstract;
using VoteMovie.Repositories.Abstract;

namespace VoteMovie.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntityBase, new()
    {
        private MoviewAppContext context;
        private DbSet<TEntity> dbSet;

        public DbSet<TEntity> Set() => dbSet;

        public BaseRepository(MoviewAppContext Context)
        {
            context = Context;
            dbSet = Context.Set<TEntity>();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Any(predicate);
        }

        public bool Exist(int id)
        {
            return context.Set<TEntity>().Any(x => x.Id == id);
        }

        public int Count()
        {
            return context.Set<TEntity>().Count();
        }

        public Task<int> CountAsync()
        {
            return context.Set<TEntity>().CountAsync();
        }

        public List<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            var t = new TEntity();
            List<string> listIncludeProps = new List<string>();

            IQueryable<TEntity> query = context.Set<TEntity>().Where(predicate);

            foreach (var includeProperty in listIncludeProps)
            {
                try
                {
                    query = query.Include(includeProperty);
                }
                catch
                {
                }
            }
            return query;
        }

        public TEntity GetByID(int id)
        {
            return context.Set<TEntity>().SingleOrDefault(x => x.Id == id);
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public TEntity GetSingleWithNoIsEnable(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(int id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityDelete)
        {
            if (context.Entry(entityDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityDelete);
            }

            dbSet.Remove(entityDelete);
        }

        public virtual void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> entities = context.Set<TEntity>().Where(predicate);
            foreach (var entity in entities)
            {
                context.Entry<TEntity>(entity).State = EntityState.Deleted;
            }
        }

        public virtual void Update(TEntity entityUpdate)
        {
            dbSet.Attach(entityUpdate);
            context.Entry(entityUpdate).State = EntityState.Modified;
        }

        public void InsertAndUpdate(TEntity entity)
        {
            if (entity.Id == 0)
                Insert(entity);
            else
                Update(entity);
        }
    }
}
