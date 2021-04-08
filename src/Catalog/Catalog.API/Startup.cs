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

            // ������������ ��������� ������������, � ������� TOptions ����� �����������.          
            services.Configure<CatalogDatabaseSettings>(Configuration.GetSection(nameof(CatalogDatabaseSettings)));

            //����������� �������
            //����� �� ����� ��������� �� ICatalogDatabaseSettings � ����� ������������, �� .Net Core ����� ��������� ���� ���, � ������� ��������� �������
            //��� ������� ������������ ��� ��������� ������������ �������
            //GetRequiredService �������� ������� ��������� ���� ��������
            services.AddSingleton<ICatalogDatabaseSettings>(sp => sp.GetRequiredService<IOptions<CatalogDatabaseSettings>>().Value);

            /* Transient - ��� ������ ��������� � ������� ��������� ����� ������ �������. 
             * � ������� ������ ������� ����� ���� ��������� ��������� � �������, 
             * �������������� ��� ������ ��������� ����� ����������� ����� ������. 
             * �������� ������ ���������� ����� �������� �������� ��� ����������� ��������, 
             * ������� �� ������ ������ ���������.
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