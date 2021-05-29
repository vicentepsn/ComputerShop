using ComputerShop.WebAPI.DataAccess.Contracts;
using ComputerShop.WebAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ComputerShop.WebAPI.DataAccess.Repositories
{
    public class LaptopRepository : RepositoryWithIdBase<Laptop>, ILaptopRepository
    {
        protected DbSet<LaptopConfiguration> LaptopConfigurationDbSet { get; }
        public LaptopRepository(ApiDbContext context) : base(context)
        {
            LaptopConfigurationDbSet = context.Set<LaptopConfiguration>();
        }

        public bool Exists(string brandName)
        {
            return DbSet.Any(_ => _.BrandName == brandName);
        }

        public IEnumerable<Laptop> GetAll()
        {
            return DbSet
                .Include(c => c.LaptopConfigurations).ThenInclude(cs => cs.Configuration);
        }

        public void AddConfigurations(int LaptopId, IEnumerable<int> ConfigurationIds)
        {
            var laptopConfigurations = ConfigurationIds.Select(_ => new LaptopConfiguration { LaptopId = LaptopId, ConfigurationId = _ });
            //foreach(var laptopConfiguration in laptopConfigurations)
            //{
            //    LaptopConfigurationDbSet.Add(laptopConfiguration);
            //}
            LaptopConfigurationDbSet.AddRange(laptopConfigurations);
        }
    }
}
