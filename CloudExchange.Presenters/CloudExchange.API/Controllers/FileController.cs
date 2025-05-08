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
    public class FileController : ControllerBase
    {
        private readonly IUserFileService _userFileService;

        private readonly IServerFileService _serverFileService;

        public FileController(IUserFileService userFileService,
                              IServerFileService serverFileService)
        {
            _userFileService = userFileService;
            _serverFileService = serverFileService;
        }

        [HttpGet]
        public async Task<IResult> Get(CancellationToken cancellation = default)
        {
            Result<IEnumerable<DescriptorEntity>> descriptorsResult = await _serverFileService.GetDescriptorsAsync(cancellation);

            return descriptorsResult.IsSuccess ?
                        Results.Ok(descriptorsResult) :
                        Results.BadRequest(descriptorsResult);
        }

        [HttpGet]
        [Route("{descriptorId}")]
        public async Task<IActionResult> Get(Guid descriptorId,
                                             string? download = null,
                                             CancellationToken cancellation = default)
        {
            Result<FileDto> fileDtoResult = await _userFileService.GetFileAsync(descriptorId,
                                                                                download,
                                                                                cancellation);

            return fileDtoResult.IsSuccess ?
                        File(fileDtoResult.Content) :
                        BadRequest(fileDtoResult);
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

            return createResult.IsSuccess ?
                        Results.Ok(createResult) :
                        Results.BadRequest(createResult);
        }

        [HttpDelete]
        public async Task<IResult> Delete([FromBody] DeleteContract contract,
                                          CancellationToken cancellation = default)
        {
            Result deleteResult = await _userFileService.DeleteFileAsync(contract.DescriptorId,
                                                                         contract.Root,
                                                                         cancellation);

            return deleteResult.IsSuccess ?
                        Results.Ok(deleteResult) :
                        Results.BadRequest(deleteResult);
        }


        private FileStreamResult File(FileDto fileDto)
        {
            return File(fileDto.Data,
                        MediaTypeNames.Application.Octet,
                        fileDto.Descriptor.Name);
        }
    }
}
