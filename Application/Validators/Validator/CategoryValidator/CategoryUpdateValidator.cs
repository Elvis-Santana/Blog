using Application.Dtos.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Validator.CategoryValidator;

public class CategoryUpdateValidator :AbstractValidator<CategoryUpdateDTO>
{
    public CategoryUpdateValidator()
    {
        RuleFor(x => x.Name)
             .MaximumLength(50)
             .WithMessage(CategoryMsg.CategoryErroNameMax)
             .NotEmpty()
             .WithMessage(CategoryMsg.CategoryErroNameNotEmpty);
    }
}
