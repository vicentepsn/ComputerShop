using System.Collections.Generic;

namespace ComputerShop.WebAPI.DTO
{
    public class BasketItemDTO
    {
        public int Id { get; set; }
        public LaptopDTO Laptop { get; set; }
        public int Quantity { get; set; }
        public IEnumerable<ConfigurationDTO> selectedConfigurations { get; set; }
    }
}
