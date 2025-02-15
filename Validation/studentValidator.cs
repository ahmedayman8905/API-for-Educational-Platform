using Api_1.Entity;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api_1.Validation;

public class studentValidator : AbstractValidator<Student>
{
    private const string Password = "(?=(.*[0-9]))(?=.*[\\!@#$%^&*()\\\\[\\]{}\\-_+=~`|:;\"'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";

    public studentValidator()
    {
        RuleFor(x => x.Email)
            
             .NotEmpty()
             .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(3, 100)
            //.Matches(Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

       

        

    }
}
 