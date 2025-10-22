using Application.Dtos.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions;

public static  class PasswordHasherExtensions
{

    public static bool Verify(this Author author, string password)
        => BCrypt.Net.BCrypt.Verify(password, author.PasswordHash);

    //public static bool Verify(this Login , string password)
    //   => BCrypt.Net.BCrypt.Verify(password, au);
    
    //public static string Hash(this Author author)
    //  => BCrypt.Net.BCrypt.HashPassword(password);

}
