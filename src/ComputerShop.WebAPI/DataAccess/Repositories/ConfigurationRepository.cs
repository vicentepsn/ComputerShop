using ComputerShop.WebAPI.DataAccess.Contracts;
using ComputerShop.WebAPI.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ComputerShop.WebAPI.DataAccess.Repositories
{
    public class ConfigurationRepository : RepositoryWithIdBase<Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(ApiDbContext context) : base(context)
        {
        }

        public IEnumerable<Configuration> GetAll()
        {
            return DbContext.Configurations;
        }

        public bool Exists(string configurationDescription)
        {
            return DbContext.Configurations.Any(_ => _.Description.Equals(configurationDescription, System.StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
