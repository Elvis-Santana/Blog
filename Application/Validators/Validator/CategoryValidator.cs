using Application.Dtos.Models;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Validators.AuthorValidator;

public class CategoryValidator: AbstractValidator<AddCategoryInputModel>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50)
            .WithMessage(CategoryMsg.CategoryErroNameMax)
            .NotEmpty()
            .WithMessage(CategoryMsg.CategoryErroNameNotEmpty);

        RuleFor(x => x.IdAuthor)
            .NotNull()
            .WithMessage(CategoryMsg.CategoryErroIdAuthorNotNull)
            .NotEmpty()
            .WithMessage(CategoryMsg.CategoryErroIdAuthorNotEmpty); ;
    }

    public override ValidationResult Validate(ValidationContext<AddCategoryInputModel> context)
    {
        return context.InstanceToValidate is null ?new ValidationResult(new[]
        {
            new ValidationFailure(nameof(AddCategoryInputModel),CategoryMsg.CategoryErroNull)
        }): base.Validate(context);
    }

    public override async Task<ValidationResult> ValidateAsync(ValidationContext<AddCategoryInputModel> context, CancellationToken cancellation = default)
    {
        return  await Task.Run(()=> context.InstanceToValidate is null)?new ValidationResult(new[]
        {
            new ValidationFailure(nameof(AddCategoryInputModel),CategoryMsg.CategoryErroNull)
        }): base.Validate(context);
    }
}
