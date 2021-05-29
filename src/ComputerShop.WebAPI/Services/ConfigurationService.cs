using ComputerShop.WebAPI.DataAccess.Contracts;
using ComputerShop.WebAPI.DataAccess.Entities;
using ComputerShop.WebAPI.DTO;
using ComputerShop.WebAPI.DTO.Extensions;
using ComputerShop.WebAPI.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ComputerShop.WebAPI.Services
{
    public class ConfigurationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfigurationRepository _configurationRepository;

        public ConfigurationService(IUnitOfWork unitOfWork, IConfigurationRepository configurationRepository)
        {
            _unitOfWork = unitOfWork;
            _configurationRepository = configurationRepository;
        }

        public int AddConfiguration(ConfigurationDTO configuration)
        {
            if (_configurationRepository.Exists(configuration.Description))
            {
                throw new HttpResponseException((int)HttpStatusCode.BadRequest, "Configuration already exists");
            }

            var newConfiguration = new Configuration
            {
                Id = configuration.Id,
                Description = configuration.Description,
                ConfigurationType = (ConfigurationType)((int)configuration.ConfigurationType),
                Cost = configuration.Cost
            };

            _unitOfWork.CleanupChanges();
            _configurationRepository.Add(newConfiguration);
            _unitOfWork.SaveChanges();

            return newConfiguration.Id;
        }

        public IEnumerable<ConfigurationDTO> GetConfigurations()
        {
            return _configurationRepository.GetAll().AsParallel().Select(_ => _.ToDto());
        }
    }
}
