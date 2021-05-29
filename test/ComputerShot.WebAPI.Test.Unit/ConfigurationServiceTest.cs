using ComputerShop.WebAPI.DataAccess;
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
    public class ConfigurationServiceTest
    {
        private readonly ConfigurationService _configurationService;
        private readonly ApiDbContext _dbContext;

        public ConfigurationServiceTest()
        {
            var builder = new DbContextOptionsBuilder<ApiDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _dbContext = new ApiDbContext(builder.Options);
            
            _configurationService = new ConfigurationService(new UnitOfWork(_dbContext), new ConfigurationRepository(_dbContext));
        }

        [Fact]
        public void AddConfiguration_passes()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            var configList = new List<ConfigurationDTO>
            {
                new ConfigurationDTO
                {
                    ConfigurationType = ConfigurationTypeDTO.Ram,
                    Description = "8GB",
                    Cost = 45.67
                },
                new ConfigurationDTO
                {
                    ConfigurationType = ConfigurationTypeDTO.Ram,
                    Description = "16GB",
                    Cost = 87.88
                },
                new ConfigurationDTO
                {
                    ConfigurationType = ConfigurationTypeDTO.HDD,
                    Description = "500GB",
                    Cost = 123.34
                },
                new ConfigurationDTO
                {
                    ConfigurationType = ConfigurationTypeDTO.HDD,
                    Description = "1TB",
                    Cost = 200.0
                },
                new ConfigurationDTO
                {
                    ConfigurationType = ConfigurationTypeDTO.Colour,
                    Description = "Red",
                    Cost = 50.76
                },
                new ConfigurationDTO
                {
                    ConfigurationType = ConfigurationTypeDTO.Colour,
                    Description = "Blue",
                    Cost = 64.56
                },
            };

            // Act
            var resultIds = new List<int>();
            foreach (var config in configList)
            {
                resultIds.Add(_configurationService.AddConfiguration(config));
            }

            // Assert
            foreach (var resultId in resultIds)
            {
                Assert.True(resultId > 0); // means that the config was correctly added and receives a new Id
            }
        }

        [Fact]
        public void AddConfiguration_fails_duplicated()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            var configRam = new ConfigurationDTO
            {
                ConfigurationType = ConfigurationTypeDTO.Ram,
                Description = "4GB",
                Cost = 25.0
            };
            var configHDD = new ConfigurationDTO
            {
                ConfigurationType = ConfigurationTypeDTO.HDD,
                Description = "256GB",
                Cost = 70.00
            };
            var configColour = new ConfigurationDTO
            {
                ConfigurationType = ConfigurationTypeDTO.Colour,
                Description = "Silver",
                Cost = 55.55
            };

            // Act / Assert
            Assert.True(_configurationService.AddConfiguration(configRam) > 0);
            Assert.True(_configurationService.AddConfiguration(configHDD) > 0);
            Assert.True(_configurationService.AddConfiguration(configColour) > 0);

            Assert.Throws<HttpResponseException>(() => _configurationService.AddConfiguration(configRam));
            Assert.Throws<HttpResponseException>(() => _configurationService.AddConfiguration(configHDD));
            Assert.Throws<HttpResponseException>(() => _configurationService.AddConfiguration(configColour));
        }

        [Fact]
        public void GetAllConfiguration_passes()
        {
            // Arrange
            _dbContext.Database.EnsureDeleted();

            var configRam = new ConfigurationDTO
            {
                ConfigurationType = ConfigurationTypeDTO.Ram,
                Description = "4GB",
                Cost = 25.0
            };
            var configHDD = new ConfigurationDTO
            {
                ConfigurationType = ConfigurationTypeDTO.HDD,
                Description = "256GB",
                Cost = 70.00
            };
            var configColour = new ConfigurationDTO
            {
                ConfigurationType = ConfigurationTypeDTO.Colour,
                Description = "Silver",
                Cost = 55.55
            };

            // Act
            _configurationService.AddConfiguration(configRam);
            _configurationService.AddConfiguration(configHDD);
            _configurationService.AddConfiguration(configColour);

            var configs = _configurationService.GetConfigurations();
            var configRamResult = configs.FirstOrDefault(_ => _.ConfigurationType == ConfigurationTypeDTO.Ram);
            var configHDDResult = configs.FirstOrDefault(_ => _.ConfigurationType == ConfigurationTypeDTO.HDD);
            var configColourResult = configs.FirstOrDefault(_ => _.ConfigurationType == ConfigurationTypeDTO.Colour);

            Assert.Equal(3, configs.Count());
            Assert.Equal(configRam.Description, configRamResult.Description);
            Assert.Equal(configHDD.Description, configHDDResult.Description);
            Assert.Equal(configColour.Description, configColourResult.Description);
        }


    }
}
