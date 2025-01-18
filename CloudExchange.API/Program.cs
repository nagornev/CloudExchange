using CloudExchange.API.Backgrounds;
using CloudExchange.API.Extensions.Startup;
using Hangfire;
using Org.BouncyCastle.Security;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services.AddContexts(configuration);
services.AddRepositories();
services.AddServices();
services.AddProviders();
services.AddValidation();
services.AddConstraints();
services.AddControllers(configuration);
services.AddOptions(configuration);
services.AddCors(configuration);
services.AddHangfires(configuration);
services.AddBackgrounds();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseHangfireDashboard("/hangfire");
app.MapControllers();

app.Run();

