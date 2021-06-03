using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Plano.BL.Repository;
using Plano.DAL.DbContext;
using Plano.DAL.Repository;
using Plano.Data.Interface;
using Plano.Data.Models;
using System.Collections.Generic;
using System.Globalization;

namespace Plano.WebAPI
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
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(
             options => {
                 var supportedCultures = new List<CultureInfo> {
                    new CultureInfo("en-SG"),
                    new CultureInfo("en-US"),
                    new CultureInfo("ja-JP"),
                    new CultureInfo("zh-CN")
                };

                 options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(culture:"en-US",uiCulture: "en-US");
                 options.SupportedCultures = supportedCultures;
                 options.SupportedUICultures = supportedCultures;
                 options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider {  } };
            });



            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddHttpClient();
            services.AddTransient<IRepository<Person>, RepositoryPerson>();
            services.AddTransient<PersonService, PersonService>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Plano.WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Plano.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            var localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizeOptions.Value);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
