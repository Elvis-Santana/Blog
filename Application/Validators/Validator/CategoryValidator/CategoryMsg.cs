using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Validator.CategoryValidator;

public sealed class CategoryMsg
{
    public const string CategoryErroIdAuthorNotEmpty = "IdAuthor não pode ser vazio";
    public const string CategoryErroIdAuthorNotNull = "IdAuthor não pode ser null";


    public const string CategoryErroNameMax = "Name não pode ser mais de 50 caracteres";

    public const string CategoryErroNameNotEmpty = "Name não pode ser vazio";

    public const string CategoryErroNull = "Category objeto não pode ser null";

}
