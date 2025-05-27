using AutoMapper;
using CloudExchange.Application.Abstractions.Providers;
using CloudExchange.Application.Dto;
using CloudExchange.Application.Extensions;
using CloudExchange.Domain.Abstractions.Providers;
using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Files.Commands.CreateFile
{
    public class CreateFileHandler(IDescriptorRepository _descriptorRepository,
                                   IDataRepository _dataRepository,
                                   IDescriptorCredentialsHashProvider _descriptorCredentialsHashProvider,
                                   IPathProvider _pathProvider,
                                   ITimeProvider _timeProvider,
                                   IMapper _mapper)
        : IRequestHandler<CreateFileCommand, Result<DescriptorDto>>
    {
        public async Task<Result<DescriptorDto>> Handle(CreateFileCommand request, CancellationToken cancellationToken)
        {
            Result<DescriptorEntity> descriptorEntityResult = DescriptorEntity.New(request.Name,
                                                                                   request.Weight,
                                                                                   _pathProvider.GetPath(),
                                                                                   _timeProvider.NowUnix(),
                                                                                   request.Lifetime,
                                                                                   request.Download,
                                                                                   request.Root,
                                                                                   _descriptorCredentialsHashProvider);

            return descriptorEntityResult.IsSuccess ?
                    await CreateFile(descriptorEntityResult.Content, request.Data, cancellationToken) :
                    Result<DescriptorDto>.Failure(descriptorEntityResult.Error);
        }

        private async Task<Result<DescriptorDto>> CreateFile(DescriptorEntity descriptor,
                                                             Stream data,
                                                             CancellationToken cancellation = default)
        {
            Result createResult = await _descriptorRepository.CreateAsync(descriptor,
                                                                          async (descriptor, cancellation) => await _dataRepository.CreateAsync(descriptor, data, cancellation),
                                                                          cancellation);

            return createResult.IsSuccess ?
                        Result<DescriptorDto>.Success(_mapper.Map<DescriptorDto>(descriptor)) :
                        Result<DescriptorDto>.Failure(createResult.Error);
        }
    }
}
