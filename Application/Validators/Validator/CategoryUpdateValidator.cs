using Application.Dtos.Models;
using Application.Validators.AuthorValidator;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Validator;

public class CategoryUpdateValidator :AbstractValidator<UpdateCategoryInputModel>
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
