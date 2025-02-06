using ChickenBroker.Application.Models.Options;
using FluentValidation;

namespace ChickenBroker.Application.Validators;

public class GetAllPropertyAgencyOptionsValidator : AbstractValidator<GetAllPropertyAgencyOptions>
{
    public GetAllPropertyAgencyOptionsValidator()
    {
        RuleFor(x => x.ZipCode).Custom((s, context) =>
        {
            if (!String.IsNullOrEmpty(s) && s.Length != 8)
            {
                context.AddFailure("Invalid ZipCode");
            }
        });
        
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal to 1.");
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).WithMessage("PageSize must be greater than or equal to 1.");
        RuleFor(x => x.PageSize).LessThanOrEqualTo(25).WithMessage("PageSize must be less than or equal to 25.");
    }
}