using ComputerShop.WebAPI.DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerShop.WebAPI.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _context;

        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChanges()
        {
            return _context.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }
        public void CleanupChanges()
        {
            foreach (var dbEntityEntry in _context.ChangeTracker.Entries().ToList())
            {

                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }
        }

    }


}

