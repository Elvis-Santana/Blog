using Application.Dtos.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Validator;

public class PostValidator :AbstractValidator<AddPostInputModel>
{
    public PostValidator()
    {
        When(x => x is not null, () =>
        {
            RuleFor(x => x.title)
            .NotEmpty()
            .WithMessage(PostMsg.postErroTitleNotEmpty)
            .MaximumLength(50)
            .WithMessage(PostMsg.postErroTitleMax);

            RuleFor(x => x.text)
            .NotEmpty()
            .WithMessage(PostMsg.postErroTextNotEmpty)
            .MaximumLength(2000)
            .WithMessage(PostMsg.postErroTextMax);

            RuleFor(x => x.authorId)
            .NotEmpty().WithMessage(PostMsg.postErroAuthorIdNotEmpty);

            RuleFor(x => x.categoryId)
            .NotEmpty().WithMessage(PostMsg.postErroCategoryIdNotEmpty);

            RuleFor(x => x.Date)
            .NotNull().WithMessage(PostMsg.postErroDataNotNull);

            

        });
    }
}
