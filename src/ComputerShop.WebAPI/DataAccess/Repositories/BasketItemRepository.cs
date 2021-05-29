using ComputerShop.WebAPI.DataAccess.Contracts;
using ComputerShop.WebAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ComputerShop.WebAPI.DataAccess.Repositories
{
    public class BasketItemRepository : RepositoryWithIdBase<BasketItem>, IBasketItemRepository
    {
        protected DbSet<BasketItemConfiguration> BasketItemConfigurationsDbSet { get; }

        public BasketItemRepository(ApiDbContext context) : base(context)
        {
            BasketItemConfigurationsDbSet = context.Set<BasketItemConfiguration>();
        }

        public bool Exists(int laptopId, IEnumerable<int> configurationIds)
        {
            var query = DbSet
                .Include(_ => _.BasketItemConfigurations).ThenInclude(_ => _.Configuration).ToList();

            var item = query
                .FirstOrDefault(_ =>
                    _.LaptopId == laptopId 
                    && _.BasketItemConfigurations.Select(_ => _.ConfigurationId).OrderBy(_ => _)
                        .SequenceEqual(configurationIds.OrderBy(_ => _)));

            return item != null;
        }

        public IEnumerable<BasketItem> GetAll()
        {
            return DbSet
                .Include(_ => _.Laptop)
                .Include(c => c.BasketItemConfigurations).ThenInclude(cs => cs.Configuration);
        }

        public void AddConfigurations(int basketItemId, IEnumerable<int> ConfigurationIds)
        {
            var basketItemConfigurations = ConfigurationIds.Select(_ => new BasketItemConfiguration { BasketItemId = basketItemId, ConfigurationId = _ });
            BasketItemConfigurationsDbSet.AddRange(basketItemConfigurations);
        }
    }
}
