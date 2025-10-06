using CloudExchange.Domain.ValueObjects;
using FluentValidation;

namespace CloudExchange.API.Contracts
{
    public class DeleteContractValidator : AbstractValidator<DeleteContract>
    {
        public DeleteContractValidator()
        {
            #region DescriptorId

            RuleFor(x => x.DescriptorId).NotNull()
                                        .NotEmpty()
                                        .WithMessage("The descriptor ID can`t be null or empty.");


            #endregion

            #region Root

            RuleFor(x => x.Root).NotNull()
                                .WithMessage("The root password can`t be null or emprty.");

            When(x => x.Root != null, () =>
            {
                RuleFor(x => x.Root).Must(root => root!.Length >= DescriptorCredentialsValueObject.RootMinimumLenght && root!.Length <= DescriptorCredentialsValueObject.RootMaximumLenght)
                                       .WithMessage($"The root password can`t be less than {DescriptorCredentialsValueObject.RootMinimumLenght} and more than {DescriptorCredentialsValueObject.RootMaximumLenght}.");
            });

            #endregion
        }
    }
}
