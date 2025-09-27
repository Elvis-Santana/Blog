using Application.Dtos.Models;
using Application.IServices;
using Domain.Entities;
using Domain.Erros;
using Domain.Erros.AppErro;
using Domain.IRepository.ICategoryRepository;
using FluentValidation;
using Mapster;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.CategoryService;

public class CategoryService (
    ICategoryRepository categoryRepository,
    IValidator<AddCategoryInputModel> addValidator,
    IValidator<UpdateCategoryInputModel> updateValidator
    ) : ICategoryService{

    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IValidator<AddCategoryInputModel> _addValidator = addValidator;
    private readonly IValidator<UpdateCategoryInputModel> _updateValidator = updateValidator;

    public async Task<OneOf<CategoryViewModel, Errors>> Create(AddCategoryInputModel addCategoryInputModel)
    {
       var result = await this._addValidator.ValidateAsync(addCategoryInputModel);

        if (!result.IsValid)
            return Errors.Factory.CreateErro(result.Errors.Select(a => new AppErro(a.ErrorMessage, a.PropertyName)));

        var category = (await this._categoryRepository.Create((Category)addCategoryInputModel));
        return category.Map();
    }

    public async Task<OneOf<bool, Errors>> DeleteById(string id)
    {
        return await this._categoryRepository.DeleteById(id);
    }

    public async Task<OneOf<List<CategoryViewModel>, Errors>> GetAsync() =>
     (await this._categoryRepository.GetAsync()).Map();
    


    public async Task<OneOf<CategoryViewModel, Errors>> GetById(string id)
    {
        return (await _categoryRepository.GetById(id)).Map();
    }

    public async Task<OneOf<CategoryViewModel, Errors>> Update(UpdateCategoryInputModel updateCategory, string id)
    {
        var result = await this._updateValidator.ValidateAsync(updateCategory);

        if (!result.IsValid)
            return  Errors.Factory.CreateErro(result.Errors.Select(a => new AppErro(a.ErrorMessage, a.PropertyName)));


        var category = await this._categoryRepository.GetById(id);
        category.UpdateName(updateCategory.Name);   

        return (await this._categoryRepository.Update(category, id)).Map();
    }
}
