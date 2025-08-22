using Application.Dtos.Models;
using Domain.Entities;
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
    Task<OneOf<bool,Errors>> Create(AddCategoryInputModel category);


    Task<OneOf<List<CategoryViewModel>, Errors>> GetAsync();

    Task<OneOf<CategoryViewModel, Errors>> GetById(string id);
    Task<OneOf<CategoryViewModel, Errors>> Update(UpdateCategoryInputModel category,string id);

    Task<OneOf<bool, Errors>> DeleteById(string id);


}
