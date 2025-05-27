using CloudExchange.OperationResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudExchange.Application.Abstractions.Services.Features.Files
{
    public interface IDeleteFileByServerService
    {
        Task<Result> DeleteFile(Guid descriptorId, CancellationToken cancellation = default);
    }
}
