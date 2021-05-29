using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShop.WebAPI.DataAccess.Entities
{
    public class BasketItem: EntityWithIdBase
    {
        //[Required]
        //[ForeignKey(nameof(Basket))]
        //public int BasketId { get; set; }

        [Required]
        [ForeignKey(nameof(Laptop))]
        public int LaptopId { get; set; }

        [Required]
        public Laptop Laptop { get; set; }

        [Required]
        public int Quantity { get; set; }

        public ICollection<BasketItemConfiguration> BasketItemConfigurations { get; set; }
    }


    public class BasketItemConfiguration
    {
        [Required]
        [ForeignKey(nameof(BasketItem))]
        public int BasketItemId { get; set; }
        public BasketItem BasketItem { get; set; }

        [Required]
        [ForeignKey(nameof(Configuration))]
        public int ConfigurationId { get; set; }
        public Configuration Configuration { get; set; }
    }

}
