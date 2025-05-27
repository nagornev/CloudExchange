using CloudExchange.Application.Dto;
using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Files.Commands.CreateFile
{
    public record CreateFileCommand(string Name,
                                    int Weight,
                                    Stream Data,
                                    int Lifetime,
                                    string? Root = null,
                                    string? Download = null) 
        : IRequest<Result<DescriptorDto>>;
}
