using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Validator;

public sealed class PostMsg
{
    public const string postErroTitleNotEmpty = "titulo no pode ser vazio";
    public const string postErroTitleMax = "o texto não pode utrapacar de 50 caracteres";

    public const string postErroTextNotEmpty = "texto não pode ser vazio";
    public const string postErroTextMax = "texto não pode ultrapassar 2000 caracteres";

    public const string postErroAuthorIdNotEmpty = "AuthorId não pode ser vazio";
    //public const string postErroCategoryIdNotEmpty = "categoryId não pode ser vazio";
    public const string postErroDataNotNull = "data não pode ser nulo";

}
