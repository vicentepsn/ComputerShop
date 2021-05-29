using ComputerShop.WebAPI.DTO;
using ComputerShop.WebAPI.Services;
using ComputerShop.WebAPI.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ComputerShop.WebAPI.Controllers
{
    [ApiController]
    [Route("v1/Configuration")]
    public class ConfigurationController : Controller
    {
        private readonly ConfigurationService _configurationService;

        public ConfigurationController(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpPut]
        public IActionResult AddConfiguration([FromBody] ConfigurationDTO configuration)
        {
            try
            {
                var newConfigurationId = _configurationService.AddConfiguration(configuration);
                return Ok(newConfigurationId);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.Status, ex.Value);
            }
        }

        [HttpGet]
        public IActionResult GetConfiguration()
        {
            try
            {
                var configurations = _configurationService.GetConfigurations();
                return Ok(configurations);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.Status, ex.Value);
            }
        }
    }
}
