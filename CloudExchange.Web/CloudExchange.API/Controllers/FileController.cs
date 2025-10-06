using CloudExchange.API.Abstractions.Providers;
using CloudExchange.API.Contracts;
using CloudExchange.Application.Dto;
using CloudExchange.Application.Features.DeleteFile;
using CloudExchange.Application.Features.DownloadFile;
using CloudExchange.Application.Features.GetDescriptors;
using CloudExchange.Application.Features.UploadFile;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OperationResults;

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
        public async Task<IResult> Download(Guid descriptorId,
                                            string? download = null,
                                            CancellationToken cancellation = default)
        {
            Result<DownloadDto> downloadResult = await _mediator.Send(new DownloadQuery(descriptorId,
                                                                                        download),
                                                                      cancellation);

            return _resultProvider.GetResult(downloadResult);
        }

        [HttpPost]
        public async Task<IResult> BeginUpload([FromBody] BeginUploadContract contract,
                                               CancellationToken cancellation = default)
        {
            Result<BeginUploadDto> beginUploadResult = await _mediator.Send(new BeginUploadFileCommand(contract.Name,
                                                                                                       contract.Weight,
                                                                                                       contract.Lifetime,
                                                                                                       contract.Root,
                                                                                                       contract.Download),
                                                                            cancellation);

            return _resultProvider.GetResult(beginUploadResult);
        }

        [HttpPut]
        public async Task<IResult> ContinueUpload([FromBody] ContinueUploadContract contract,
                                                  CancellationToken cancellation = default)
        {
            Result<ContinueUploadDto> continueUploadResult = await _mediator.Send(new ContinueUploadFileCommand(contract.Id,
                                                                                                                contract.Key,
                                                                                                                contract.Part),
                                                                                  cancellation);

            return _resultProvider.GetResult(continueUploadResult);
        }

        [HttpPatch]
        public async Task<IResult> CompleteUpload([FromBody] CompleteUploadContract contract,
                                                  CancellationToken cancellation = default)
        {
            Result completeUploadResult = await _mediator.Send(new CompleteUploadFileCommand(contract.Id,
                                                                                             contract.Key,
                                                                                             contract.Parts),
                                                               cancellation);

            return _resultProvider.GetResult(completeUploadResult);
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
