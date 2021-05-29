using ComputerShop.WebAPI.DataAccess.Entities;
using System.Collections.Generic;

namespace ComputerShop.WebAPI.DataAccess.Contracts
{
    public interface ILaptopRepository : IRepositoryWithIdBase<Laptop>
    {
        bool Exists(string brandName);
        IEnumerable<Laptop> GetAll();
        void AddConfigurations(int LaptopId, IEnumerable<int> ConfigurationIds);
    }
}