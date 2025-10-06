using CloudExchange.Application.Abstractions.Services;
using MassTransit;
using MessageContracts;
using OperationResults;

namespace CloudExchange.Messaging.Consumers
{
    public class DescriptorDeletedConsumer : IConsumer<DescriptorDeletedMessageContract>
    {
        private readonly IDescriptorDeletedEventService _descriptorDeletedEventService;

        public DescriptorDeletedConsumer(IDescriptorDeletedEventService descriptorDeletedEventService)
        {
            _descriptorDeletedEventService = descriptorDeletedEventService;
        }

        public async Task Consume(ConsumeContext<DescriptorDeletedMessageContract> context)
        {
            Result handleResult = await _descriptorDeletedEventService.HandleAsync(context.Message.DescriptorId,
                                                                                   context.Message.Name,
                                                                                   context.Message.Upload);

            if (handleResult.IsFailure)
                throw new InvalidOperationException(handleResult.Error.Message);
        }
    }
}
