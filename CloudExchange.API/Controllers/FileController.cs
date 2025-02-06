using CloudExchange.API.Contracts;
using CloudExchange.API.Extensions;
using CloudExchange.Domain.Models;
using CloudExchange.OperationResults;
using CloudExchange.UseCases.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using File = CloudExchange.Domain.Models.File;

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
        public async Task<IResult> Get()
        {
            Result<IEnumerable<Descriptor>> descriptorsResult = await _serverFileService.GetDescriptors();

            return descriptorsResult.Success ?
                        Results.Ok(descriptorsResult) :
                        Results.BadRequest(descriptorsResult);
        }

        [HttpGet]
        [Route("{descriptorId}")]
        public async Task<IActionResult> Get(Guid descriptorId, string? download = null)
        {
            Result<File> fileResult = await _userFileService.GetFile(descriptorId, download);

            return fileResult.Success ?
                        File(fileResult.Content) :
                        BadRequest(fileResult);
        }

        [HttpPost]
        public async Task<IResult> Create([FromForm] CreateContract contract)
        {
            Result<Descriptor> createResult = await _userFileService.CreateFile(contract.File.GetName(),
                                                                                contract.File.GetWeight(),
                                                                                contract.File.GetData(),
                                                                                contract.Lifetime,
                                                                                contract.Root,
                                                                                contract.Download);

            return createResult.Success ?
                        Results.Ok(createResult) :
                        Results.BadRequest(createResult);
        }

        [HttpDelete]
        public async Task<IResult> Delete([FromBody] DeleteContract contract)
        {
            Result deleteResult = await _userFileService.DeleteFile(contract.DescriptorId, contract.Root);

            return deleteResult.Success ?
                        Results.Ok(deleteResult) :
                        Results.BadRequest(deleteResult);
        }


        private FileStreamResult File(File file)
        {
            return File(file.Data,
                        MediaTypeNames.Application.Octet,
                        file.Descriptor.Name);
        }
    }
}
