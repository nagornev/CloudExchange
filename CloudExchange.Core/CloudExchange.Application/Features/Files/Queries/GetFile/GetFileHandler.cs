using AutoMapper;
using CloudExchange.Application.Dto;
using CloudExchange.Domain.Abstractions.Providers;
using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Entities;
using CloudExchange.Domain.Failures;
using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Files.Queries.GetFile
{
    public class GetFileHandler(IDescriptorRepository _descriptorRepository,
                                IDataRepository _dataRepository,
                                IDescriptorCredentialsHashProvider _descriptorCredentialsHashProvider,
                                IMapper _mapper)
        : IRequestHandler<GetFileQuery, Result<FileDto>>
    {
        public async Task<Result<FileDto>> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            Result<DescriptorEntity> descriptorEntityResult = await IsDownloadAllowed(request.DescriptorId, 
                                                                                      request.Download,
                                                                                      cancellationToken);

            return descriptorEntityResult.IsSuccess ?
                    await GetFile(descriptorEntityResult.Content, cancellationToken) :
                    Result<FileDto>.Failure(descriptorEntityResult.Error);
        }

        private async Task<Result<DescriptorEntity>> IsDownloadAllowed(Guid descriptorId,
                                                                       string? download,
                                                                       CancellationToken cancellation = default)
        {
            Result<DescriptorEntity> descriptorEntityResult = await _descriptorRepository.GetAsync(descriptorId, cancellation);

            if (descriptorEntityResult.IsSuccess &&
                descriptorEntityResult.Content.Credentials?.Download != null)
                return _descriptorCredentialsHashProvider.Verify(download, descriptorEntityResult.Content.Credentials.Download) ?
                            descriptorEntityResult :
                            Result<DescriptorEntity>.Failure(Errors.InvalidDownload("Invalid download password."));

            return descriptorEntityResult;
        }

        private async Task<Result<FileDto>> GetFile(DescriptorEntity descriptorEntity,
                                                    CancellationToken cancellation = default)
        {
            Result<Stream> dataResult = await _dataRepository.GetAsync(descriptorEntity, cancellation);

            return dataResult.IsSuccess ?
                    Result<FileDto>.Success(new FileDto(_mapper.Map<DescriptorDto>(descriptorEntity), dataResult.Content)):
                    Result<FileDto>.Failure(dataResult.Error);
        }
    }
}
