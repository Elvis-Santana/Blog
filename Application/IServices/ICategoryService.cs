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
    Task<OneOf<CategoryReadDTO, Errors>> CreateCategory(CategoryCreateDTO category);

    Task<IEnumerable<CategoryReadDTO> > GetAllCategoryAsync();

    Task<OneOf<CategoryReadDTO, Errors>> GetCategoryByIdAsync(string id);

    Task<OneOf<CategoryReadDTO, Errors>> UpdateCategoryAsync(CategoryUpdateDTO category,string id);

    Task<OneOf<bool, Errors>> RemoveCategoryByIdAsync(string id);


}
