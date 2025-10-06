using CloudExchange.Domain.Aggregates;
using CloudExchange.Domain.ValueObjects;
using FluentValidation;

namespace CloudExchange.API.Contracts
{
    public class BeginUploadContractValidator : AbstractValidator<BeginUploadContract>
    {
        public BeginUploadContractValidator()
        {
            

            #region Name

            RuleFor(x => x.Name).NotNull()
                                .WithMessage("The file name can`t be null.");

            RuleFor(x => x.Name).NotEmpty()
                                .WithMessage("The file name can`t be empty.");

            #endregion

            #region Weight

            RuleFor(x => x.Weight).NotNull()
                                  .WithMessage("The file weight can`t be null.");

            RuleFor(x => x.Weight).NotEmpty()
                                  .WithMessage("The file weight can`t be empty.");

            When(x => x.Weight != null, () =>
            {
                RuleFor(x => x.Weight).Must(x =>  x <= DescriptorAggregate.WeightMaximum)
                                      .WithMessage($"The file weight can`t be more than {DescriptorAggregate.WeightMaximum} bytes.");
            });

            #endregion

            #region Lifetime

            RuleFor(x => x.Lifetime).Must(x => x >= DescriptorAggregate.LifetimeMinumum && x <= DescriptorAggregate.LifetimeMaximum)
                                    .WithMessage($"The file lifetime can`t be less than {DescriptorAggregate.LifetimeMinumum} and more than {DescriptorAggregate.LifetimeMaximum} seconds.");

            #endregion

            #region Root

            When(x => !string.IsNullOrEmpty(x.Root), () =>
            {
                RuleFor(x => x.Root).Must(root => root!.Length >= DescriptorCredentialsValueObject.RootMinimumLenght && root!.Length <= DescriptorCredentialsValueObject.RootMaximumLenght)
                                       .WithMessage($"The root password can`t be less than {DescriptorCredentialsValueObject.RootMinimumLenght} and more than {DescriptorCredentialsValueObject.RootMaximumLenght}.");
            });

            #endregion

            #region Download

            When(x => !string.IsNullOrEmpty(x.Download), () =>
            {
                RuleFor(x => x.Download).Must(download => download!.Length >= DescriptorCredentialsValueObject.DownloadMinimumLenght && download!.Length <= DescriptorCredentialsValueObject.DownloadMaximumLenght)
                                        .WithMessage($"The download password can`t be less than {DescriptorCredentialsValueObject.DownloadMinimumLenght} and more than {DescriptorCredentialsValueObject.DownloadMaximumLenght}.");
            });

            #endregion
        }
    }
}
