using CloudExchange.API.Abstractions.Providers;
using CloudExchange.API.Contracts;
using CloudExchange.API.Extensions;
using CloudExchange.Application.Dto;
using CloudExchange.Application.Features.Descriptors.Queries.GetAllDescriptors;
using CloudExchange.Application.Features.Files.Commands.CreateFile;
using CloudExchange.Application.Features.Files.Commands.DeleteFileByUser;
using CloudExchange.Application.Features.Files.Queries.GetFile;
using CloudExchange.OperationResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CloudExchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController
    {
        private readonly IMediator _mediator;

        private readonly IResultProvider _resultProvider;

        public FileController(IMediator mediator,
                              IResultProvider resultProvider)
        {
            _mediator = mediator;
            _resultProvider = resultProvider;
        }

        [HttpGet]
        public async Task<IResult> Get(CancellationToken cancellation = default)
        {
            Result<IEnumerable<DescriptorDto>> descriptorsResult = await _mediator.Send(new GetAllDescriptorsQuery(),
                                                                                        cancellation);

            return _resultProvider.GetResult(descriptorsResult);
        }

        [HttpGet]
        [Route("{descriptorId}")]
        public async Task<IResult> Get(Guid descriptorId,
                                       string? download = null,
                                       CancellationToken cancellation = default)
        {
            Result<FileDto> fileResult = await _mediator.Send(new GetFileQuery(descriptorId,
                                                                               download),
                                                              cancellation);

            return _resultProvider.GetResult(fileResult);
        }

        [HttpPost]
        public async Task<IResult> Create([FromForm] CreateContract contract,
                                          CancellationToken cancellation = default)
        {
            Result<DescriptorDto> createResult = await _mediator.Send(new CreateFileCommand(contract.File.GetName(),
                                                                                            contract.File.GetWeight(),
                                                                                            contract.File.GetData(),
                                                                                            contract.Lifetime,
                                                                                            contract.Root,
                                                                                            contract.Download),
                                                                      cancellation);

            return _resultProvider.GetResult(createResult);
        }

        [HttpDelete]
        public async Task<IResult> Delete([FromBody] DeleteContract contract,
                                          CancellationToken cancellation = default)
        {
            Result deleteResult = await _mediator.Send(new DeleteFileByUserCommand(contract.DescriptorId,
                                                                                   contract.Root),
                                                       cancellation);

            return _resultProvider.GetResult(deleteResult);
        }
    }
}
