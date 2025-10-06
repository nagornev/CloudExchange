using CloudExchange.Application.Abstractions.Services;
using CloudExchange.Application.Abstractions.Validators;
using CloudExchange.Domain.Aggregates;
using DDD.Repositories;
using OperationResults;

namespace CloudExchange.Application.Services
{
    public class DeleteFileService : IDeleteFileService
    {
        private readonly IDescriptorQueryService _descriptorQueryService;

        private readonly IDescriptorCredentialsValidator _descriptorCredentialsValidator;

        private readonly IRepositoryWriter<DescriptorAggregate> _descriptorRepository;

        private readonly IUnitOfWork _unitOfWork;

        public DeleteFileService(IDescriptorQueryService descriptorQueryService,
                                 IDescriptorCredentialsValidator descriptorCredentialsValidator,
                                 IRepositoryWriter<DescriptorAggregate> descriptorRepository,
                                 IUnitOfWork unitOfWork)
        {
            _descriptorQueryService = descriptorQueryService;
            _descriptorCredentialsValidator = descriptorCredentialsValidator;
            _descriptorRepository = descriptorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> DeleteAsync(Guid descriptorId, string root, CancellationToken cancellation = default)
        {
            Result<DescriptorAggregate> descriptorResult = await IsRootAllowed(descriptorId, root, cancellation);

            if (descriptorResult.IsFailure)
                return descriptorResult;

            DescriptorAggregate descriptor = descriptorResult.Content;

            descriptor.MarkAsDeleted();
            await _descriptorRepository.DeleteAsync(descriptor);
            await _unitOfWork.SaveAsync(cancellation);

            return Result.Success();
        }

        private async Task<Result<DescriptorAggregate>> IsRootAllowed(Guid descriptorId,
                                                                      string root,
                                                                      CancellationToken cancellation = default)
        {
            Result<DescriptorAggregate> descriptorResult = await _descriptorQueryService.GetDescriptorByIdAsync(descriptorId, cancellation);

            if (descriptorResult.IsSuccess &&
                descriptorResult.Content.Credentials?.RootHash != null)
                return !string.IsNullOrEmpty(root)?
                            _descriptorCredentialsValidator.Validate(root,
                                                                     descriptorResult.Content.Credentials.Salt!,
                                                                     descriptorResult.Content.Credentials.RootHash) ?
                                descriptorResult :
                                Result<DescriptorAggregate>.Failure(ResultError.InvalidRoot("Invalid root password.")):
                            Result<DescriptorAggregate>.Failure(ResultError.InvalidRoot("Invalid root password."));

            return descriptorResult.IsFailure ?
                        descriptorResult :
                        Result<DescriptorAggregate>.Failure(ResultError.NullOrEmpty("This file does`t have a root password."));
        }
    }
}
