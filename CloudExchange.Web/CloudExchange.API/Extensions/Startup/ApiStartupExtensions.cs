using Hangfire;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ApiStartupExtensions
    {
        public static WebApplication CloudExchange(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddOptions(configuration);
            services.AddApplication();
            services.AddInfrastructure(configuration);
            services.AddWeb();
            services.AddBackgrounds();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return builder.Build();
        }

        public static void Start(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseHangfireDashboard();
            app.MapControllers();

            app.Run();
        }
    }
}
