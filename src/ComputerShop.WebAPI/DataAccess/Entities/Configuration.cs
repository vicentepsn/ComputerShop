using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComputerShop.WebAPI.DataAccess.Entities
{
    public class Configuration: EntityWithIdBase
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public ConfigurationType ConfigurationType { get; set; }

        [Required]
        [Range(0.01, Double.MaxValue, ErrorMessage = "The Cost must be greater than 0.")]
        public double Cost { get; set; }

        public ICollection<LaptopConfiguration> LaptopConfigurations { get; set; }
        public ICollection<BasketItemConfiguration> BasketItemConfigurations { get; set; }
    }
}
