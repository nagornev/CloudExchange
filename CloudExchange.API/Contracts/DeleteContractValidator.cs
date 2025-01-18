using CloudExchange.Domain.Models;
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

            #region MyRegion


            RuleFor(x => x.Root).NotNull()
                                .WithMessage("The root password can`t be null or emprty.");

            When(x => x.Root != null, () =>
            {
                RuleFor(x => x.Root).Must(root => root!.Length >= Descriptor.RootMinimumLenght && root!.Length <= Descriptor.RootMaximumLenght)
                                       .WithMessage($"The root password can`t be less than {Descriptor.RootMinimumLenght} and more than {Descriptor.RootMaximumLenght}.");
            });

            #endregion
        }
    }
}
