using ComputerShop.WebAPI.DataAccess;
using ComputerShop.WebAPI.DataAccess.Contracts;
using ComputerShop.WebAPI.DataAccess.Repositories;
using ComputerShop.WebAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ComputerShop.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApiDbContext>(opt => opt.UseInMemoryDatabase("ComputerShopDb"), ServiceLifetime.Singleton, ServiceLifetime.Singleton);

            services
                .AddControllers()
                .AddNewtonsoftJson();

            services
                .AddSingleton<IUnitOfWork, UnitOfWork>()
                .AddScoped<IConfigurationRepository, ConfigurationRepository>()
                .AddScoped<ILaptopRepository, LaptopRepository>()
                .AddScoped<IBasketItemRepository, BasketItemRepository>()
                .AddScoped<ConfigurationService>()
                .AddScoped<LaptopService>()
                .AddScoped<BasketService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
