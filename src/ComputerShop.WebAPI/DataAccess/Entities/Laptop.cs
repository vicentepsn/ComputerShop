using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShop.WebAPI.DataAccess.Entities
{
    public class Laptop : EntityWithIdBase
    {
        //[Required]
        //[ForeignKey(nameof(Brand))]
        //public int BrandId { get; set; }

        //[Required]
        //public virtual Brand Brand { get; set; }

        [Required]
        public string BrandName { get; set; }

        [Required]
        [Range(0.01, Double.MaxValue, ErrorMessage = "The Cost must be greater than 0.")]
        public double BrandCost { get; set; }

        public ICollection<LaptopConfiguration> LaptopConfigurations { get; set; }
    }

    public class LaptopConfiguration
    {
        [Required]
        [ForeignKey(nameof(Laptop))]
        public int LaptopId { get; set; }
        public Laptop Laptop { get; set; }

        [Required]
        [ForeignKey(nameof(Configuration))]
        public int ConfigurationId { get; set; }
        public Configuration Configuration { get; set; }
    }
}
