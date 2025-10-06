using MassTransit;
using MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.Messaging.Consumers
{
    public class RawJsonConsumer : IConsumer<RawJsonMessageContract>
    {
        public Task Consume(ConsumeContext<RawJsonMessageContract> context)
        {
            var message = context.Message.Body;

            return Task.CompletedTask;
        }
    }
}
