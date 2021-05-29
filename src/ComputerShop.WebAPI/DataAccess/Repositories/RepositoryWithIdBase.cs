using ComputerShop.WebAPI.DataAccess.Contracts;
using ComputerShop.WebAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ComputerShop.WebAPI.DataAccess.Repositories
{
    public abstract class RepositoryWithIdBase<TEntity> : IRepositoryWithIdBase<TEntity> where TEntity : EntityWithIdBase
    {
        protected ApiDbContext DbContext { get; private set; }
        protected DbSet<TEntity> DbSet { get; }

        protected RepositoryWithIdBase(ApiDbContext context)
        {
            DbContext = context;
            DbSet = context.Set<TEntity>();
        }

        public void Add(TEntity obj)
        {
            DbSet.Add(obj);
        }

        public Task<TEntity> GetById(int id)
        {
            var query = DbSet;

            return query.FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// </summary>
        /// <remarks>Marked as virtual to allow derived classes to implement logical deletion. The derived classes can just throw a exception or reimplement running a update on a IsActive flag.</remarks>
        /// <param name="obj"></param>
        public virtual void Remove(TEntity obj)
        {
            DbSet.Remove(obj);
        }

        public void Update(TEntity obj)
        {
            DbSet.Update(obj);
        }

        public void AddOrUpdate(TEntity obj)
        {
            if (obj.Id == 0)
                Add(obj);
            else
                Update(obj);
        }
    }
}
