using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Messaging.Buses;
using MassTransit;
using MassTransit.AmazonSqsTransport;
using MessageContracts;

namespace CloudExchange.Messaging.Consumers
{
    public class S3FileUploadCompletedConsumer : IConsumer<S3FileUploadCompletedMessageContract>
    {
        private readonly IBus _bus;

        private readonly IStorageKeyProvider _storageKeyProvider;

        public S3FileUploadCompletedConsumer(IBus bus,
                                             IStorageKeyProvider storageKeyProvider)
        {
            _bus = bus;
            _storageKeyProvider = storageKeyProvider;
        }

        public async Task Consume(ConsumeContext<S3FileUploadCompletedMessageContract> context)
        {
            if (context.Message.Records == null)
                return;

            foreach (var record in context.Message.Records)
            {
                Guid descriptorId = _storageKeyProvider.GetId(record.S3.Object.Key);

                await _bus.Publish(new FileUploadCompletedMessageContract(descriptorId));

            }
        }
    }
}
