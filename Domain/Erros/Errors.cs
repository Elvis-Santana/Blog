using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Erros;

public record Errors {
   

    public List<AppErro.AppErro> errors {  get; private set ;  }
    private Errors(IEnumerable<AppErro.AppErro> errors)
    {
        this.errors = errors.ToList();
    }

    public static class Factory
    {
        public static Errors CreateErro(IEnumerable<AppErro.AppErro> errors)
           =>  new Errors(errors);
        
        
    }


}



