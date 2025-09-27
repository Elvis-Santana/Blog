using Domain.Entities;
using Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public record AddAuthorInputModel(FullName Name) {

 
    public static explicit operator Author(AddAuthorInputModel addAuthorInputModel)
     => Author.Factory.CriarAuthor(addAuthorInputModel.Name);
     
    
}


