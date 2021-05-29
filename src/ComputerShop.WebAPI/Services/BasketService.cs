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
    public class BasketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILaptopRepository _laptopRepository;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IBasketItemRepository _basketItemRepository;

        public BasketService(IUnitOfWork unitOfWork, ILaptopRepository laptopRepository, IConfigurationRepository configurationRepository, IBasketItemRepository basketItemRepository)
        {
            _unitOfWork = unitOfWork;
            _laptopRepository = laptopRepository;
            _configurationRepository = configurationRepository;
            _basketItemRepository = basketItemRepository;
        }

        public int AddBasketItem(BasketItemDTO basketItem)
        {
            if (_basketItemRepository.Exists(basketItem.Laptop.Id, basketItem.selectedConfigurations.Select(_ => _.Id)))
            {
                throw new HttpResponseException((int)HttpStatusCode.BadRequest, "A Laptop with same selected configurations was already included");
            }

            var selectedConfigurations = from storedConfig in _configurationRepository.GetAll()
                                          join config in basketItem.selectedConfigurations
                                            on storedConfig.Id equals config.Id
                                          select storedConfig;

            ValidateNewBanketItemConfigurtions(selectedConfigurations);

            var newBasketItem = new BasketItem
            {
                LaptopId = basketItem.Laptop.Id,
                Quantity = basketItem.Quantity,
            };

            _unitOfWork.CleanupChanges();
            _basketItemRepository.Add(newBasketItem);
            _unitOfWork.SaveChanges();
            _unitOfWork.CleanupChanges();
            _basketItemRepository.AddConfigurations(newBasketItem.Id, selectedConfigurations.Select(_ => _.Id));
            _unitOfWork.SaveChanges();

            return newBasketItem.Id;
        }

        private void ValidateNewBanketItemConfigurtions(IEnumerable<Configuration> configurations)
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

            if (configurations.Count() > configurationTypesCount)
            {
                throw new HttpResponseException((int)HttpStatusCode.BadRequest, $"One or more configuration types are duplicated");
            }
        }

        public IEnumerable<BasketItemDTO> GetBasketItems()
        {
            return _basketItemRepository.GetAll().AsParallel().Select(_ => _.ToDto());
        }
    }
}
