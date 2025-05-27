using CloudExchange.OperationResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.Application.Features.Files.Commands.DeleteExpiredFile
{
    public record DeleteFileByServerCommand(Guid DescriptorId) : IRequest<Result>;
}
