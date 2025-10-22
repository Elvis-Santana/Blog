using Application.Dtos.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Validator.PostValidator;

public class PostUpdateValidator : AbstractValidator<PostUpdateDTO>
{

    public PostUpdateValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(50)
            .WithMessage(PostMsg.postErroTitleMax);


        RuleFor(x => x.Text)
           .MaximumLength(2000)
           .WithMessage(PostMsg.postErroTextMax);
    }
    
}
