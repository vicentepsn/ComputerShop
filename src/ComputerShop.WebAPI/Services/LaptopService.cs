using ComputerShop.WebAPI.DataAccess.Contracts;
using ComputerShop.WebAPI.DataAccess.Entities;
using ComputerShop.WebAPI.DTO;
using ComputerShop.WebAPI.DTO.Extensions;
using ComputerShop.WebAPI.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ComputerShop.WebAPI.Services
{
    public class LaptopService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILaptopRepository _laptopRepository;
        private readonly IConfigurationRepository _configurationRepository;

        public LaptopService(IUnitOfWork unitOfWork, ILaptopRepository laptopRepository, IConfigurationRepository configurationRepository)
        {
            _unitOfWork = unitOfWork;
            _laptopRepository = laptopRepository;
            _configurationRepository = configurationRepository;
        }

        public int AddLaptop(LaptopDTO laptop)
        {
            if (_laptopRepository.Exists(laptop.BrandName))
            {
                throw new HttpResponseException((int)HttpStatusCode.BadRequest, "Laptop brand already exists");
            }
            if (!(laptop.AvailableConfigurations?.Any() ?? false))
            {
                throw new HttpResponseException((int)HttpStatusCode.BadRequest, $"Configurations not informed");
            }

            var availableConfigurations = from storedConfig in _configurationRepository.GetAll()
                                    join config in laptop.AvailableConfigurations
                                      on storedConfig.Id equals config.Id
                                    select storedConfig;

            ValidateNewLaptopConfigurtions(availableConfigurations);

            var newLaptop = new Laptop
            {
                BrandName = laptop.BrandName,
                BrandCost = laptop.BrandCost,
            };

            _unitOfWork.CleanupChanges();
            _laptopRepository.Add(newLaptop);
            _unitOfWork.SaveChanges();
            _unitOfWork.CleanupChanges();
            _laptopRepository.AddConfigurations(newLaptop.Id, availableConfigurations.Select(_ => _.Id));
            _unitOfWork.SaveChanges();

            return newLaptop.Id;
        }

        private void ValidateNewLaptopConfigurtions(IEnumerable<Configuration> configurations)
        {
            var newDistinctConfigurationsCount = configurations.Select(_ => _.Id).Distinct().Count();

            if (configurations.Count() > newDistinctConfigurationsCount)
            {
                throw new HttpResponseException((int)HttpStatusCode.BadRequest, $"One or more configuration are duplicated");
            }
            
            var configurationTypesCount = Enum.GetNames(typeof(ConfigurationTypeDTO)).Length;

            var newDistinctConfigurationTypesCount = configurations.Select(_ => _.ConfigurationType).Distinct().Count();

            if (configurationTypesCount > newDistinctConfigurationTypesCount)
            {
                throw new HttpResponseException((int)HttpStatusCode.BadRequest, $"One or more configuration types are missing");
            }
        }

        public IEnumerable<LaptopDTO> GetLaptops()
        {
            return _laptopRepository.GetAll().AsParallel().Select(_ => _.ToDto());
        }
    }
}
