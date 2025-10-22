using Application.Dtos.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Validators.Validator.CategoryValidator;

public class CategoryCreateValidator: AbstractValidator<CategoryCreateDTO>
{
    public CategoryCreateValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .WithMessage(CategoryMsg.CategoryErroNameMax)
            .NotEmpty()
            .WithMessage(CategoryMsg.CategoryErroNameNotEmpty);

        RuleFor(x => x.AuthorId)
            .NotNull()
            .WithMessage(CategoryMsg.CategoryErroIdAuthorNotNull)
            .NotEmpty()
            .WithMessage(CategoryMsg.CategoryErroIdAuthorNotEmpty); ;
    }

    public override ValidationResult Validate(ValidationContext<CategoryCreateDTO> context)
    {
        return context.InstanceToValidate is null ?new ValidationResult(new[]
        {
            new ValidationFailure(nameof(CategoryCreateDTO),CategoryMsg.CategoryErroNull)
        }): base.Validate(context);
    }

    public override async Task<ValidationResult> ValidateAsync(ValidationContext<CategoryCreateDTO> context, CancellationToken cancellation = default)
    {
        return  await Task.Run(()=> context.InstanceToValidate is null)?new ValidationResult(new[]
        {
            new ValidationFailure(nameof(CategoryCreateDTO),CategoryMsg.CategoryErroNull)
        }): base.Validate(context);
    }
}
