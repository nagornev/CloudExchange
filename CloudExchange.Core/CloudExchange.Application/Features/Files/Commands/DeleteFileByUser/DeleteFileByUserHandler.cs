using CloudExchange.Domain.Abstractions.Providers;
using CloudExchange.Domain.Abstractions.Repositories;
using CloudExchange.Domain.Entities;
using CloudExchange.Domain.Failures;
using CloudExchange.OperationResults;
using MediatR;

namespace CloudExchange.Application.Features.Files.Commands.DeleteFileByUser
{
    public class DeleteFileByUserHandler(IDescriptorRepository _descriptorRepository,
                                         IDataRepository _dataRepository,
                                         IDescriptorCredentialsHashProvider _descriptorCredentialsHashProvider)
        : IRequestHandler<DeleteFileByUserCommand, Result>
    {
        public async Task<Result> Handle(DeleteFileByUserCommand request, CancellationToken cancellationToken)
        {
            Result<DescriptorEntity> descriptorEntityResult = await IsRootAllowed(request.DescriptorId, request.Root, cancellationToken);

            return descriptorEntityResult.IsSuccess ?
                    await DeleteFile(descriptorEntityResult.Content, cancellationToken) :
                    descriptorEntityResult;
        }

        private async Task<Result<DescriptorEntity>> IsRootAllowed(Guid descriptorId,
                                                                   string root,
                                                                   CancellationToken cancellation = default)
        {
            Result<DescriptorEntity> descriptorResult = await _descriptorRepository.GetAsync(descriptorId, cancellation);

            if (descriptorResult.IsSuccess &&
                descriptorResult.Content.Credentials.Root != null)
                return _descriptorCredentialsHashProvider.Verify(root, descriptorResult.Content.Credentials.Root) ?
                            descriptorResult :
                            Result<DescriptorEntity>.Failure(Errors.InvalidRoot("Invalid root password."));

            return descriptorResult.IsFailure ?
                    descriptorResult :
                    Result<DescriptorEntity>.Failure(Errors.InvalidRoot("This file does`t have a root password."));
        }

        private async Task<Result> DeleteFile(DescriptorEntity descriptor,
                                              CancellationToken cancellation = default)
        {
            return await _descriptorRepository.DeleteAsync(descriptor,
                                                           async (descriptor, cancellation) => await _dataRepository.DeleteAsync(descriptor, cancellation),
                                                           cancellation);
        }

    }
}
