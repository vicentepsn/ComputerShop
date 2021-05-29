using System.Threading.Tasks;

namespace ComputerShop.WebAPI.DataAccess.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();

        void CleanupChanges();
    }
}
