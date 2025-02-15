using Api_1.Entity;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api_1.Validation;

public class ReConfirm_emailval : AbstractValidator<ReConfirm_email>
{
    private const string Password = "(?=(.*[0-9]))(?=.*[\\!@#$%^&*()\\\\[\\]{}\\-_+=~`|:;\"'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";

    public ReConfirm_emailval()
    {
        RuleFor(x => x.Email)
            
             .NotEmpty()
             .EmailAddress();

       
    }
}
 