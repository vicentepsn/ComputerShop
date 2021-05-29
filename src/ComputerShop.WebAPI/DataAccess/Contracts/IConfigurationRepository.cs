using ComputerShop.WebAPI.DataAccess.Entities;
using System.Collections.Generic;

namespace ComputerShop.WebAPI.DataAccess.Contracts
{
    public interface IConfigurationRepository : IRepositoryWithIdBase<Configuration>
    {
        IEnumerable<Configuration> GetAll();
        bool Exists(string configurationDescription);
    }
}