using Domain.Entities;
using Domain.Enums;
using Domain.Erros.AppErro;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Erros;

public sealed record Errors {
   

    public  IReadOnlyCollection<AppErro.AppErro> errors {  get; }
    private Errors(IEnumerable<AppErro.AppErro> errors)
    {
        this.errors = errors.ToList();
    }

    
     public static Errors CreateError(IEnumerable<AppErro.AppErro> errors)=>  new (errors);



    public static bool TryValid( FluentValidation.Results.ValidationResult result, out Errors errors)
    {

        errors = CreateError( result.Errors.Select(a => new AppErro.AppErro(a.ErrorMessage, a.PropertyName ) ));
        return !result.IsValid;
    }

    public static Errors EmiteError(string message ,string field) => CreateError( [new AppErro.AppErro(message, field)] );







}



