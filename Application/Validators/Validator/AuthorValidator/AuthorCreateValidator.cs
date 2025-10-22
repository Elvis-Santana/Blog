using Application.Dtos.Models;
using Domain.Entities;
using Domain.ObjectValues;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Validator.AuthorValidator;

public class AuthorCreateValidator : AbstractValidator<AuthorCreateDTO>
{
    public AuthorCreateValidator()
    {

        When(x => x != null, () =>
        {

            RuleFor(x => x.Name.FirstName)
                .NotEmpty().WithMessage(AuthorMsg.FirstNameErroEmpty)
                .MaximumLength(100).WithMessage(AuthorMsg.FirstNameErroMaximumLength);

            RuleFor(x => x.Name.LastName)
                .MaximumLength(100).WithMessage(AuthorMsg.LastNameErroMaximumLength);
        });

        

    }
    public override ValidationResult Validate(ValidationContext<AuthorCreateDTO> context)
    {
        return context.InstanceToValidate is null
            ?new ValidationResult (new[] { new ValidationFailure(nameof(AuthorCreateDTO), AuthorMsg.AuthorErroNull) })
            :base.Validate(context);
    }

    public override async Task<ValidationResult> ValidateAsync(ValidationContext<AuthorCreateDTO> context, CancellationToken cancellation = default)
    {
        return await Task.Run(() => context.InstanceToValidate is null
             ? new ValidationResult(new[] { new ValidationFailure(nameof(AuthorCreateDTO), AuthorMsg.AuthorErroNull) })
             : base.Validate(context));
    }

}
