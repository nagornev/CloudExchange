using FluentValidation;

namespace CloudExchange.API.Contracts
{
    public class CompleteUploadContractValidator : AbstractValidator<CompleteUploadContract>
    {
        public CompleteUploadContractValidator()
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

            #region Parts

            RuleFor(x => x.Parts).NotNull()
                                 .WithMessage("The upload parts can`t be null.");

            RuleFor(x => x.Parts).NotEmpty()
                                 .WithMessage("The upload parts can`t be empty.");

            #endregion
        }
    }
}
