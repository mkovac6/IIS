using Marko_Kovacevic_iis1.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marko_Kovacevic_iis1
{
    internal class Startup
    {
        //internal static PredavacArray PredavacArray;
        private List<PredavacArray> listPredavac;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            listPredavac = new List<PredavacArray>
            {
                new PredavacArray
                {
                    Id = "1",
                    Type = "Visi predavac",
                    Name = "Marko",
                    Placa = 8000
                },
                new PredavacArray
                {
                    Id = "1",
                    Type = "Voditelj studija",
                    Name = "Ana",
                    Placa = 10000
                },
                new PredavacArray
                {
                    Id = "1",
                    Type = "Predavac",
                    Name = "Maja",
                    Placa = 7000
                }
            };
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddXmlDataContractSerializerFormatters();

            services.AddSingleton<List<PredavacArray>>(listPredavac);
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
