using Domain.Entities;
using Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public record AuthorCreateDTO {

     public FullName Name;
     public string Password;
     public string Email;

    public AuthorCreateDTO(FullName name, string password, string email)
    {
        Name = name;
        Password = BCrypt.Net.BCrypt.HashPassword(password);
        Email = email;
    }
}


