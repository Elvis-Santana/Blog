using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Validator.AuthorValidator;

public sealed class AuthorMsg
{
    public const string FirstNameErroEmpty = "FirstName não pode ser vazio";
    public const string FirstNameErroMaximumLength = "FirstName não pode ser maior que 100";

    public const string LastNameErroMaximumLength = "LastName não pode ser maior que 100";
    public const string AuthorErroNull = "Author objeto não pode ser null";

}
