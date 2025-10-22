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
    IValidator<CategoryCreateDTO> addValidator,
    IValidator<CategoryUpdateDTO> updateValidator
    ) : ICategoryService{

    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IValidator<CategoryCreateDTO> _addValidator = addValidator;
    private readonly IValidator<CategoryUpdateDTO> _updateValidator = updateValidator;

    public async Task<OneOf<CategoryReadDTO, Errors>> Create(CategoryCreateDTO addCategoryInputModel)
    {
       var result = await this._addValidator.ValidateAsync(addCategoryInputModel);

        if (!result.IsValid)
            return Errors.Factory.CreateErro(result.Errors.Select(a => new AppErro(a.ErrorMessage, a.PropertyName)));

        var category = (await this._categoryRepository.Create((Category)addCategoryInputModel));
        return category.Map();
    }

    public async Task<OneOf<bool, Errors>> DeleteById(string id)
    {
        var result = await this._categoryRepository.DeleteById(id);
        if(!result)
            return Errors.Factory.CreateErro([new AppErro("Category not found", nameof(Category))]);
        return result;
    }

    public async Task<OneOf<List<CategoryReadDTO>, Errors>> GetAsync() =>
     (await this._categoryRepository.GetAsync()).Map();
    


    public async Task<OneOf<CategoryReadDTO, Errors>> GetById(string id)
    {
        var  result = (await _categoryRepository.GetById(id));

        if(result is null)
            return Errors.Factory.CreateErro([new AppErro("Category not found", nameof(Category))]);

        return result.Map();


    }

    public async Task<OneOf<CategoryReadDTO, Errors>> Update(CategoryUpdateDTO updateCategory, string id)
    {
        var resultTest = await this._updateValidator.ValidateAsync(updateCategory);

        if (!resultTest.IsValid)
            return  Errors.Factory.CreateErro(resultTest.Errors.Select(a => new AppErro(a.ErrorMessage, a.PropertyName)));


        var category = await this._categoryRepository.GetById(id);



        if (category is null)
            return Errors.Factory.CreateErro([new AppErro("Category not found", nameof(Category))]);

        category.UpdateName(updateCategory.Name);

        var result = await this._categoryRepository.Update(category, id);

        return result.Map();
    }
}
