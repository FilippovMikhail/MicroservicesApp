using Catalog.API.Data;
using Catalog.API.Data.Interfaces;
using Catalog.API.Repositories;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Catalog.API
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

            services.AddControllers();

            // Регистрирует экземпляр конфигурации, с которым TOptions будет связываться.          
            services.Configure<CatalogDatabaseSettings>(Configuration.GetSection(nameof(CatalogDatabaseSettings)));

            //Регистрация сервиса
            //Когда мы видим настройки БД ICatalogDatabaseSettings в любом конструкторе, то .Net Core будет запускать этот код, и создаст экземпляр объекта
            //Эта команда используется для получения необходимого сервиса
            //GetRequiredService означает создать экземпляр этих объектов
            services.AddSingleton<ICatalogDatabaseSettings>(sp => sp.GetRequiredService<IOptions<CatalogDatabaseSettings>>().Value);

            /* Transient - при каждом обращении к сервису создается новый объект сервиса. 
             * В течение одного запроса может быть несколько обращений к сервису, 
             * соответственно при каждом обращении будет создаваться новый объект. 
             * Подобная модель жизненного цикла наиболее подходит для легковесных сервисов, 
             * которые не хранят данных состояния.
             */
            services.AddTransient<ICatalogContext, CatalogContext>();
            services.AddTransient<IProductRepository, ProductRepository>();

            services.AddSwaggerGen(_ =>
            {
                _.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });
            });
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

            app.UseSwagger()
                .UseSwaggerUI(_ =>
                {
                    _.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1");
                });
        }
    }
}
