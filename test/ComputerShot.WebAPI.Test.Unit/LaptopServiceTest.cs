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
    public class LaptopServiceTest
    {
        private readonly ConfigurationRepository _configurationRepository;
        private readonly LaptopService _laptopService;
        private readonly ApiDbContext _dbContext;

        public LaptopServiceTest()
        {
            var builder = new DbContextOptionsBuilder<ApiDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _dbContext = new ApiDbContext(builder.Options);

            var unitOfWork = new UnitOfWork(_dbContext);
            _configurationRepository = new ConfigurationRepository(_dbContext);

            _laptopService = new LaptopService(unitOfWork, new LaptopRepository(_dbContext), _configurationRepository);
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

        [Fact]
        public void AddLaptop_passes()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            AddConfigurations();

            var newLaptop = new LaptopDTO
            {
                BrandName = "HP",
                BrandCost = 456.76,
                AvailableConfigurations = _dbContext.Configurations.Select(_ => new ConfigurationDTO { Id = _.Id })
            };

            // Act
            var newLaptopId = _laptopService.AddLaptop(newLaptop);

            // Assert
            Assert.True(newLaptopId > 0);
        }

        [Fact]
        public void AddLaptop_fail_brandDuplicted()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            AddConfigurations();

            var newLaptop = new LaptopDTO
            {
                BrandName = "HP",
                BrandCost = 456.76,
                AvailableConfigurations = _dbContext.Configurations.Select(_ => new ConfigurationDTO { Id = _.Id })
            };

            var newLaptopId = _laptopService.AddLaptop(newLaptop);

            // Act/Assert
            Assert.Throws<HttpResponseException>(() => _laptopService.AddLaptop(newLaptop));
        }

        [Fact]
        public void AddLaptop_fail_congiguraionsNotInformed()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            AddConfigurations();

            var newLaptop = new LaptopDTO
            {
                BrandName = "HP",
                BrandCost = 456.76,
            };

            // Act/Assert
            Assert.Throws<HttpResponseException>(() => _laptopService.AddLaptop(newLaptop));
        }

        [Fact]
        public void AddLaptop_fail_configurationTypeMissing()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            AddConfigurations();

            var newLaptop = new LaptopDTO
            {
                BrandName = "HP",
                BrandCost = 456.76,
                AvailableConfigurations = _dbContext.Configurations.Where(_ => _.ConfigurationType != ConfigurationType.Colour).Select(_ => new ConfigurationDTO { Id = _.Id })
            };

            // Act / Assert
            Assert.Throws<HttpResponseException>(() => _laptopService.AddLaptop(newLaptop));
        }

        [Fact]
        public void AddLaptop_fail_configurationDuplicated()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            AddConfigurations();
            var availableConfigurations = _dbContext.Configurations.Select(_ => new ConfigurationDTO { Id = _.Id }).ToList();
            availableConfigurations.Add(new ConfigurationDTO { Id = _dbContext.Configurations.First().Id });

            var newLaptop = new LaptopDTO
            {
                BrandName = "HP",
                BrandCost = 456.76,
                AvailableConfigurations = availableConfigurations.Select(_ => new ConfigurationDTO { Id = _.Id })
            };

            // Act / Assert
            Assert.Throws<HttpResponseException>(() => _laptopService.AddLaptop(newLaptop));
        }
    }
}
