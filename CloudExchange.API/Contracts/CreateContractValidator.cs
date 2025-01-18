using CloudExchange.Domain.Models;
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
                                    .Must(x => x.Length < Descriptor.WeightMaximum)
                                    .WithMessage($"The file lenght can`t be more than {Descriptor.WeightMaximum} bytes.");
            });

            #endregion

            #region Lifetime

            RuleFor(x => x.Lifetime).Must(x => x > 0 && x <= Descriptor.LifetimeMaximum)
                                    .WithMessage($"The file lifetime can`t be less than 0 and more than {Descriptor.LifetimeMaximum} seconds.");

            #endregion

            #region Root

            When(x => x.Root != null, () =>
            {
                RuleFor(x => x.Root).Must(root => root!.Length >= Descriptor.RootMinimumLenght && root!.Length <= Descriptor.RootMaximumLenght)
                                       .WithMessage($"The root password can`t be less than {Descriptor.RootMinimumLenght} and more than {Descriptor.RootMaximumLenght}.");
            });

            #endregion

            #region Download

            When(x => x.Download != null, () =>
            {
                RuleFor(x => x.Download).Must(download => download!.Length >= Descriptor.DownloadMinimumLenght && download!.Length <= Descriptor.DownloadMaximumLenght)
                                        .WithMessage($"The download password can`t be less than {Descriptor.DownloadMinimumLenght} and more than {Descriptor.DownloadMaximumLenght}.");
            });

            #endregion
        }
    }
}
