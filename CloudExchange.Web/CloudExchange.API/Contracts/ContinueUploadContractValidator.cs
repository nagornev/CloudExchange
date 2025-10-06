using FluentValidation;

namespace CloudExchange.API.Contracts
{
    public class ContinueUploadContractValidator : AbstractValidator<ContinueUploadContract>
    {
        public ContinueUploadContractValidator()
        {
            #region Id

            RuleFor(x => x.Id).NotNull()
                              .WithMessage("The upload id can`t be null.");

            RuleFor(x => x.Id).NotEmpty()
                              .WithMessage("The upload id can`t be empty.");

            #endregion


            #region Key

            RuleFor(x => x.Key).NotNull()
                               .WithMessage("The upload key can`t be null.");

            RuleFor(x => x.Key).NotEmpty()
                               .WithMessage("The upload key can`t be empty.");

            #endregion

            #region Part

            RuleFor(x => x.Part).NotNull()
                                .WithMessage("The upload part can`t be null.");

            RuleFor(x => x.Part).NotEmpty()
                                .WithMessage("The upload part can`t be empty.");

            #endregion
        }
    }
}
