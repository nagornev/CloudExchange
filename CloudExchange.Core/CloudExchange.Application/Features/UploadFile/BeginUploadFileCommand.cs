using CloudExchange.Application.Dto;
using MediatR;
using OperationResults;

namespace CloudExchange.Application.Features.UploadFile
{
    public record BeginUploadFileCommand(string Name,
                                         long Weight,
                                         int Lifetime,
                                         string? Root = null,
                                         string? Download = null)
        : IRequest<Result<BeginUploadDto>>;
}
