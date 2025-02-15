using FluentValidation;

namespace Api_1.Entity.user;

public class AddUserValidation : AbstractValidator<AddUser>
{
    public AddUserValidation()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .Length(3, 200);

        RuleFor(x => x.Email)

             .NotEmpty()
             .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(3, 100)
            //.Matches(Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

        RuleFor(x => x.Roles)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Roles)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("You cannot add duplicated Roles ")
            .When(x => x.Roles != null);
    }
}
