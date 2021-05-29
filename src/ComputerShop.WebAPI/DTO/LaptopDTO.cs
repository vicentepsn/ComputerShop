using System.Collections.Generic;

namespace ComputerShop.WebAPI.DTO
{
    public class LaptopDTO
    {
        public int Id { get; set; }
        //public BrandDTO Brand { get; set; }
        public string BrandName { get; set; }
        public double BrandCost { get; set; }
        public IEnumerable<ConfigurationDTO> AvailableConfigurations { get; set; }
    }
}
