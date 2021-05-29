using ComputerShop.WebAPI.DataAccess.Entities;
using System.Collections.Generic;

namespace ComputerShop.WebAPI.DataAccess.Contracts
{
    public interface IBasketItemRepository : IRepositoryWithIdBase<BasketItem>
    {
        bool Exists(int LaptopId, IEnumerable<int> configurationIds);
        IEnumerable<BasketItem> GetAll();
        void AddConfigurations(int basketItemId, IEnumerable<int> ConfigurationIds);
    }
}
