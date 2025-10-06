using CloudExchange.Application.Abstractions.Services;
using MassTransit;
using MessageContracts;
using OperationResults;

namespace CloudExchange.Messaging.Consumers
{
    public class FileUploadCompletedConsumer : IConsumer<FileUploadCompletedMessageContract>
    {
        private readonly IFileUploadCompletedEventService _fileUploadCompletedEventService;

        public FileUploadCompletedConsumer(IFileUploadCompletedEventService fileUploadCompletedEventService)
        {
            _fileUploadCompletedEventService = fileUploadCompletedEventService;
        }

        public async Task Consume(ConsumeContext<FileUploadCompletedMessageContract> context)
        {
            Result handleResult = await _fileUploadCompletedEventService.HandleAsync(context.Message.DescriptorId,
                                                                                     context.CancellationToken);

            if (handleResult.IsFailure)
                throw new InvalidOperationException(handleResult.Error.Message);
        }
    }
}
