using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public record CategoryCreateDTO(string AuthorId, string Name) {



   

    public static explicit operator Category(CategoryCreateDTO addCategoryInputModel)
        => Category.Factory.CreateCategory(addCategoryInputModel.AuthorId, addCategoryInputModel.Name);




}


