namespace ComputerShop.WebAPI.DTO
{
    public class ConfigurationDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ConfigurationTypeDTO ConfigurationType { get; set; }
        public double Cost { get; set; }
    }
}
