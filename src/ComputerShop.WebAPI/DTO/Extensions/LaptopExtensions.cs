using ComputerShop.WebAPI.DataAccess.Entities;
using System.Linq;

namespace ComputerShop.WebAPI.DTO.Extensions
{
    public static class LaptopExtensions
    {
        public static ConfigurationDTO ToDto(this Configuration configuration)
        {
            return new ConfigurationDTO
            {
                Id = configuration.Id,
                ConfigurationType = (ConfigurationTypeDTO)configuration.ConfigurationType,
                Description = configuration.Description,
                Cost = configuration.Cost
            };
        }

        public static LaptopDTO ToDto(this Laptop laptop)
        {
            return new LaptopDTO
            {
                BrandName = laptop.BrandName,
                Id = laptop.Id,
                BrandCost = laptop.BrandCost,
                AvailableConfigurations = laptop.LaptopConfigurations?.Select(_ => _.Configuration.ToDto())
            };
        }

        public static BasketItemDTO ToDto(this BasketItem basketItem)
        {
            return new BasketItemDTO
            {
                Id = basketItem.Id,
                Quantity = basketItem.Quantity,
                Laptop = new LaptopDTO
                {
                    Id = basketItem.LaptopId,
                    BrandName = basketItem.Laptop.BrandName,
                    BrandCost = basketItem.Laptop.BrandCost,
                },
                selectedConfigurations = basketItem.BasketItemConfigurations.Select(_ => _.Configuration.ToDto()),
            };
        }
    }
}
