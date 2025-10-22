using Application.Dtos.Models;
using Domain.Erros;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices;

public interface ICategoryService
{
    Task<OneOf<CategoryReadDTO, Errors>> Create(CategoryCreateDTO category);


    Task<OneOf<List<CategoryReadDTO>, Errors>> GetAsync();

    Task<OneOf<CategoryReadDTO, Errors>> GetById(string id);
    Task<OneOf<CategoryReadDTO, Errors>> Update(CategoryUpdateDTO category,string id);

    Task<OneOf<bool, Errors>> DeleteById(string id);


}
