using Marko_Kovacevic_iis.Model;
using static Marko_Kovacevic_iis.Model.PredavacArray;

namespace Marko_Kovacevic_iis
{
    internal class Startup
    {
        internal static PredavacArray PredavacArray;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            List<Predavac> predavac = new List<Predavac>();
            PredavacArray = new PredavacArray(predavac);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(Options => {
                Options.AddPolicy("Cors", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.AddControllers().AddXmlDataContractSerializerFormatters();

            services.AddMvc();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RESTApiIIS v1"));
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
