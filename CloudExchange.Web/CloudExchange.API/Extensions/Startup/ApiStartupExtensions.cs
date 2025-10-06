using CloudExchange.Persistence;
using CloudExchange.Persistence.Contexts;
using Hangfire;

namespace CloudExchange.API.Extensions.Startup
{
    public static class ApiStartupExtensions
    {
        public static WebApplication CloudExchange(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddOptions(configuration)
                    .AddRepositories(configuration)
                    .AddServices(configuration)
                    .AddProviders(configuration)
                    .AddBackgrounds(configuration)
                    .AddClients(configuration)
                    .AddFactories(configuration)
                    .AddValidators(configuration)
                    .AddWeb()

                    .AddEndpointsApiExplorer()
                    .AddSwaggerGen();

            return builder.Build();
        }

        public static async Task StartApplicationAsync(this WebApplication app)
        {
            await SeedAsync(app);

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

        private static async Task SeedAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                await DescriptorContextSeeder.SeedAsync(scope.ServiceProvider.GetRequiredService<DescriptorContext>());
            }
        }
    }
}
