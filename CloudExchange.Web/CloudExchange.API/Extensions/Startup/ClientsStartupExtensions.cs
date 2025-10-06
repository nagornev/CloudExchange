using Amazon;
using Amazon.S3;
using Amazon.S3.Util;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using CloudExchange.Messaging.Buses;
using CloudExchange.Messaging.Consumers;
using CloudExchange.Messaging.Options;
using CloudExchange.Persistence.Abstractions.Clients;
using CloudExchange.Persistence.Clients;
using CloudExchange.Persistence.Options;
using MassTransit;
using MessageContracts;
using Microsoft.AspNetCore.Builder;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace CloudExchange.API.Extensions.Startup
{
    public static class MessageBrokerStartupExtensions
    {
        public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
        {
            RabbitMessageBrokerOptions rabbitOptions = configuration.GetSection(nameof(RabbitMessageBrokerOptions))
                                                                    .Get<RabbitMessageBrokerOptions>()!;

            AmazonS3Options amazonS3Options = configuration.GetSection(nameof(AmazonS3Options))
                                                      .Get<AmazonS3Options>()!;

            services.AddRabbit(rabbitOptions)
                    .AddSqs(amazonS3Options)
                    .AddAmazonS3Client();

            return services;
        }
        
        private static IServiceCollection AddRabbit(this IServiceCollection services, RabbitMessageBrokerOptions options)
        {
            return services.AddMassTransit(x =>
            {
                x.AddConsumer<FileUploadCompletedConsumer>();
                x.AddConsumer<DescriptorDeletedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(options.Host, options.Port, "/", h =>
                    {
                        h.Username(options.Username);
                        h.Password(options.Password);
                    });

                    cfg.ReceiveEndpoint("cloudexchange-file-upload-completed-queue", e =>
                    {
                        e.ConfigureConsumer<FileUploadCompletedConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("cloudexchange-descriptor-deleted-queue", e =>
                    {
                        e.ConfigureConsumer<DescriptorDeletedConsumer>(context);
                    });
                });
            });
        }

        private static IServiceCollection AddSqs(this IServiceCollection services, AmazonS3Options options)
        {
            return services.AddMassTransit<ISqsBus>(x =>
            {
                x.AddConsumer<S3FileUploadCompletedConsumer>();

                x.UsingAmazonSqs((context, cfg) =>
                {
                    cfg.Host(options.Host, h =>
                    {
                        h.AccessKey(options.AccessKey);
                        h.SecretKey(options.SecretKey);

                        h.Config(new AmazonSQSConfig
                        {
                            ServiceURL = $"http://{options.Host}:{options.Port}",
                            UseHttp = true
                        });

                        h.Config(new AmazonSimpleNotificationServiceConfig
                        {
                            ServiceURL = $"http://{options.Host}:{options.Port}",
                            UseHttp = true
                        });

                        h.Scope(options.Scope);
                    });

                    cfg.ReceiveEndpoint("file-events", e =>
                    {
                        e.DefaultContentType = new ContentType("application/json");
                        e.ConfigureConsumeTopology = false;
                        e.UseRawJsonSerializer();
                        e.ConfigureConsumer<S3FileUploadCompletedConsumer>(context);
                    });
                });
            });
        }
        

        private static IServiceCollection AddAmazonS3Client(this IServiceCollection services)
        {
            return services.AddSingleton<IInternalAmazonS3, InternalAmazonS3>()
                           .AddSingleton<IExternalAmazonS3, ExternalAmazonS3>();
        }
    }
}
