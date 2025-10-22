using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Token
{

    public string token { get; init; }

    private Token(string token) 
    { 
       this.token = token;
    }

    public static class Factory
    {
       public static Token CreateToken(string token) => new Token(token);
             
        
    }
}
