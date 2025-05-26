using CloudExchange.API.Abstractions.Providers;
using CloudExchange.API.Contracts;
using CloudExchange.API.Extensions;
using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Domain.Dto;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CloudExchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController
    {
        private readonly IUserFileService _userFileService;

        private readonly IServerFileService _serverFileService;

        private readonly IResultProvider _resultProvider;

        public FileController(IUserFileService userFileService,
                              IServerFileService serverFileService,
                              IResultProvider resultProvider)
        {
            _userFileService = userFileService;
            _serverFileService = serverFileService;
            _resultProvider = resultProvider;
        }

        [HttpGet]
        public async Task<IResult> Get(CancellationToken cancellation = default)
        {
            Result<IEnumerable<DescriptorEntity>> descriptorsResult = await _serverFileService.GetDescriptorsAsync(cancellation);

            return _resultProvider.GetResult(descriptorsResult);
        }

        [HttpGet]
        [Route("{descriptorId}")]
        public async Task<IResult> Get(Guid descriptorId,
                                       string? download = null,
                                       CancellationToken cancellation = default)
        {
            Result<FileDto> fileDtoResult = await _userFileService.GetFileAsync(descriptorId,
                                                                                download,
                                                                                cancellation);

            return _resultProvider.GetResult(fileDtoResult);
        }

        [HttpPost]
        public async Task<IResult> Create([FromForm] CreateContract contract,
                                          CancellationToken cancellation = default)
        {
            Result<DescriptorEntity> createResult = await _userFileService.CreateFileAsync(contract.File.GetName(),
                                                                                           contract.File.GetWeight(),
                                                                                           contract.File.GetData(),
                                                                                           contract.Lifetime,
                                                                                           contract.Root,
                                                                                           contract.Download,
                                                                                           cancellation);

            return _resultProvider.GetResult(createResult); 
        }

        [HttpDelete]
        public async Task<IResult> Delete([FromBody] DeleteContract contract,
                                          CancellationToken cancellation = default)
        {
            Result deleteResult = await _userFileService.DeleteFileAsync(contract.DescriptorId,
                                                                         contract.Root,
                                                                         cancellation);

            return _resultProvider.GetResult(deleteResult);
        }
    }
}
