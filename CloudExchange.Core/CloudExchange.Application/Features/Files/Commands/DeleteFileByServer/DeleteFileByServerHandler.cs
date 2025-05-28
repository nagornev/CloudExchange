using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Entities;
using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Files.Commands.DeleteExpiredFile
{
    public class DeleteFileByServerHandler(IDescriptorRepository _descriptorRepository,
                                           IDataRepository _dataRepository)
        : IRequestHandler<DeleteFileByServerCommand, Result>
    {
        public async Task<Result> Handle(DeleteFileByServerCommand request, CancellationToken cancellationToken)
        {
            Result<DescriptorEntity> descriptorEntityResult = await _descriptorRepository.GetAsync(request.DescriptorId, cancellationToken);

            return descriptorEntityResult.IsSuccess ?
                    await DeleteFile(descriptorEntityResult.Content, cancellationToken) :
                    descriptorEntityResult;

        }

        private async Task<Result> DeleteFile(DescriptorEntity descriptorEntity,
                                              CancellationToken cancellation = default)
        {
            return await _descriptorRepository.DeleteAsync(descriptorEntity,
                                                           async (descriptor, cancellation) => await _dataRepository.DeleteAsync(descriptor, cancellation),
                                                           cancellation);
        }
    }
}
