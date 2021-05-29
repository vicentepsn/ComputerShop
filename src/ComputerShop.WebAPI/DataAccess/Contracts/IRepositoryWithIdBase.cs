using ComputerShop.WebAPI.DataAccess.Entities;
using System.Threading.Tasks;

namespace ComputerShop.WebAPI.DataAccess.Contracts
{
    public interface IRepositoryWithIdBase<TEntity> where TEntity : EntityWithIdBase
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(int id);
        void Update(TEntity obj);
        void Remove(TEntity obj);
        void AddOrUpdate(TEntity obj);
    }
}
