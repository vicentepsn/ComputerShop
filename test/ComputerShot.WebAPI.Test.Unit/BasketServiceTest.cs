using ComputerShop.WebAPI.DataAccess;
using ComputerShop.WebAPI.DataAccess.Entities;
using ComputerShop.WebAPI.DataAccess.Repositories;
using ComputerShop.WebAPI.DTO;
using ComputerShop.WebAPI.Services;
using ComputerShop.WebAPI.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ComputerShot.WebAPI.Test.Unit
{
    public class BasketServiceTest
    {
        private readonly ConfigurationService _configurationService;
        private readonly ConfigurationRepository _configurationRepository;
        private readonly LaptopService _laptopService;
        private readonly BasketService _basketService;
        private readonly ApiDbContext _dbContext;

        public BasketServiceTest()
        {
            var builder = new DbContextOptionsBuilder<ApiDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _dbContext = new ApiDbContext(builder.Options);

            var unitOfWork = new UnitOfWork(_dbContext);
            _configurationRepository = new ConfigurationRepository(_dbContext);
            var _laptopRepository = new LaptopRepository(_dbContext);

            _configurationService = new ConfigurationService(unitOfWork, _configurationRepository);
            _laptopService = new LaptopService(unitOfWork, _laptopRepository, _configurationRepository);

            _basketService = new BasketService(unitOfWork, _laptopRepository, _configurationRepository, new BasketItemRepository(_dbContext));
        }

        private void AddConfigurations()
        {
            var configList = new List<Configuration>
            {
                new Configuration
                {
                    ConfigurationType = ConfigurationType.Ram,
                    Description = "8GB",
                    Cost = 45.67
                },
                new Configuration
                {
                    ConfigurationType = ConfigurationType.Ram,
                    Description = "16GB",
                    Cost = 87.88
                },
                new Configuration
                {
                    ConfigurationType = ConfigurationType.HDD,
                    Description = "500GB",
                    Cost = 123.34
                },
                new Configuration
                {
                    ConfigurationType = ConfigurationType.HDD,
                    Description = "1TB",
                    Cost = 200.0
                },
                new Configuration
                {
                    ConfigurationType = ConfigurationType.Colour,
                    Description = "Red",
                    Cost = 50.76
                },
                new Configuration
                {
                    ConfigurationType = ConfigurationType.Colour,
                    Description = "Blue",
                    Cost = 64.56
                },
            };
            _dbContext.Configurations.AddRange(configList);
            _dbContext.SaveChanges();
        }

        private void AddLaptop()
        {
            var configurtions = _configurationService.GetConfigurations().Select(_ => new ConfigurationDTO { Id = _.Id });
            var laptopList = new List<LaptopDTO>
            {
                new LaptopDTO
                {
                    BrandName = "Dell",
                    BrandCost = 349.87,
                    AvailableConfigurations = configurtions
                },
                new LaptopDTO
                {
                    BrandName = "Toshiba",
                    BrandCost = 345.67,
                    AvailableConfigurations = configurtions
                },
                new LaptopDTO
                {
                    BrandName = "HP",
                    BrandCost = 456.76,
                    AvailableConfigurations = configurtions
                },
            };

            foreach (var laptop in laptopList)
                _laptopService.AddLaptop(laptop);
        }

        [Fact]
        public void AddBasketItem_passes()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            AddConfigurations();
            AddLaptop();

            var configurations = _configurationRepository.GetAll();

            var selectedConfiguration = new List<ConfigurationDTO>
            {
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.Colour).Id
                },
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.HDD).Id
                },
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.Ram).Id
                },
            };
            var newBasketItem = new BasketItemDTO
            {
                Laptop = new LaptopDTO { Id = 1 },
                Quantity = 2,
                selectedConfigurations = selectedConfiguration
            };

            // Act
            var newLaptopId = _basketService.AddBasketItem(newBasketItem);

            // Assert
            Assert.True(newLaptopId > 0);
        }

        [Fact]
        public void AddBasketItem_fail_duplicatedItem()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            AddConfigurations();
            AddLaptop();

            var configurations = _configurationRepository.GetAll();

            var selectedConfiguration = new List<ConfigurationDTO>
            {
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.Colour).Id
                },
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.HDD).Id
                },
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.Ram).Id
                },
            };
            var newBasketItem = new BasketItemDTO
            {
                Laptop = new LaptopDTO { Id = 1 },
                Quantity = 2,
                selectedConfigurations = selectedConfiguration
            };

            _basketService.AddBasketItem(newBasketItem);

            // Act/Assert
            Assert.Throws<HttpResponseException>(() => _basketService.AddBasketItem(newBasketItem));
        }

        [Fact]
        public void AddBasketItem_fail_configurationDuplicated()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            AddConfigurations();
            AddLaptop();

            var configurations = _configurationRepository.GetAll();

            var selectedConfiguration = new List<ConfigurationDTO>
            {
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.Colour).Id
                },
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.HDD).Id
                },
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.Ram).Id
                },
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.Ram).Id
                },
            };
            var newBasketItem = new BasketItemDTO
            {
                Laptop = new LaptopDTO { Id = 1 },
                Quantity = 2,
                selectedConfigurations = selectedConfiguration
            };

            // Act/Assert
            Assert.Throws<HttpResponseException>(() => _basketService.AddBasketItem(newBasketItem));
        }

        [Fact]
        public void AddBasketItem_fail_configurationTypeMissing()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            AddConfigurations();
            AddLaptop();

            var configurations = _configurationRepository.GetAll();

            var selectedConfiguration = new List<ConfigurationDTO>
            {
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.Colour).Id
                },
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.HDD).Id
                },
            };
            var newBasketItem = new BasketItemDTO
            {
                Laptop = new LaptopDTO { Id = 1 },
                Quantity = 2,
                selectedConfigurations = selectedConfiguration
            };

            // Act/Assert
            Assert.Throws<HttpResponseException>(() => _basketService.AddBasketItem(newBasketItem));
        }

        [Fact]
        public void AddBasketItem_fail_configurationTypeDuplicated()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            AddConfigurations();
            AddLaptop();

            var configurations = _configurationRepository.GetAll();

            var selectedConfiguration = new List<ConfigurationDTO>
            {
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.Colour).Id
                },
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.HDD).Id
                },
                new ConfigurationDTO
                {
                    Id = configurations.First(_ => _.ConfigurationType == ConfigurationType.Ram).Id
                },
                new ConfigurationDTO
                {
                    Id = configurations.Last(_ => _.ConfigurationType == ConfigurationType.Ram).Id
                },
            };
            var newBasketItem = new BasketItemDTO
            {
                Laptop = new LaptopDTO { Id = 1 },
                Quantity = 2,
                selectedConfigurations = selectedConfiguration
            };

            // Act/Assert
            Assert.Throws<HttpResponseException>(() => _basketService.AddBasketItem(newBasketItem));
        }
    }
}
