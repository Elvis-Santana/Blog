using Domain.Entities;
using Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public record AuthorCreateDTO(FullName Name, string password) {

 
   public static explicit operator Author(AuthorCreateDTO addAuthorInputModel) => 
    Author.Factory.CriarAuthor(
        addAuthorInputModel.Name, 
        BCrypt.Net.BCrypt.HashPassword(addAuthorInputModel.password)
    );


    
    
     
    
}


