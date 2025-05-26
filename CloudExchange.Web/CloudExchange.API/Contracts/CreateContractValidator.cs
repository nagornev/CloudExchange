using CloudExchange.Domain.Entities;
using CloudExchange.Domain.ValueObjects;
using FluentValidation;

namespace CloudExchange.API.Contracts
{
    public class CreateContractValidator : AbstractValidator<CreateContract>
    {
        public CreateContractValidator()
        {
            #region File

            RuleFor(x => x.File).NotNull()
                                .WithMessage("The file can`t be null.");


            When(x => x.File != null, () =>
            {
                RuleFor(x => x.File).Must(x => !string.IsNullOrEmpty(x.FileName) && !string.IsNullOrWhiteSpace(x.FileName))
                                    .WithMessage("The file name can`t be null or empty.")
                                    .Must(x => x.Length < DescriptorEntity.WeightMaximum)
                                    .WithMessage($"The file lenght can`t be more than {DescriptorEntity.WeightMaximum} bytes.");
            });

            #endregion

            #region Lifetime

            RuleFor(x => x.Lifetime).Must(x => x >= DescriptorEntity.LifetimeMinumum && x <= DescriptorEntity.LifetimeMaximum)
                                    .WithMessage($"The file lifetime can`t be less than {DescriptorEntity.LifetimeMinumum} and more than {DescriptorEntity.LifetimeMaximum} seconds.");

            #endregion

            #region Root

            When(x => x.Root != null, () =>
            {
                RuleFor(x => x.Root).Must(root => root!.Length >= DescriptorCredentialsValueObject.RootMinimumLenght && root!.Length <= DescriptorCredentialsValueObject.RootMaximumLenght)
                                       .WithMessage($"The root password can`t be less than {DescriptorCredentialsValueObject.RootMinimumLenght} and more than {DescriptorCredentialsValueObject.RootMaximumLenght}.");
            });

            #endregion

            #region Download

            When(x => x.Download != null, () =>
            {
                RuleFor(x => x.Download).Must(download => download!.Length >= DescriptorCredentialsValueObject.DownloadMinimumLenght && download!.Length <= DescriptorCredentialsValueObject.DownloadMaximumLenght)
                                        .WithMessage($"The download password can`t be less than {DescriptorCredentialsValueObject.DownloadMinimumLenght} and more than {DescriptorCredentialsValueObject.DownloadMaximumLenght}.");
            });

            #endregion
        }
    }
}
