using CloudExchange.API.Extensions.Startup;

var app = WebApplication.CreateBuilder(args)
                        .CloudExchange();

app.Start();

