using Application.Dtos.Models;
using Application.IServices;
using Domain.Entities;
using Domain.Erros;
using Domain.Erros.AppErro;
using Domain.IRepository.ICategoryRepository;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.UnitOfWork;
using Mapster;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.CategoryService;

public class CategoryService (
    ICategoryRepository categoryRepository,
    IValidator<CategoryCreateDTO> addValidator,
    IValidator<CategoryUpdateDTO> updateValidator,
    IUnitOfWork unitOfWork
    ) : ICategoryService{

    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IValidator<CategoryCreateDTO> _addValidator = addValidator;
    private readonly IValidator<CategoryUpdateDTO> _updateValidator = updateValidator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<OneOf<CategoryReadDTO, Errors>> CreateCategory(CategoryCreateDTO categoryCreateDTO)
    {

        if (Errors.TryValid(await this._addValidator.ValidateAsync(categoryCreateDTO), out Errors errors))
            return errors;

         var category  =(Category)categoryCreateDTO;

        await this._categoryRepository.CreateCategoryAsync(category);
        await unitOfWork.SaveAsync();
        return category.Map();
    }

    public async Task<OneOf<bool, Errors>> RemoveCategoryByIdAsync(string id)
    {
        var result = await this._categoryRepository.GetCategoryByIdAsync(id);
        if (result is null)
            return Errors.EmiteError("Category not found", nameof(Category));

        this._categoryRepository.RemoveCategoryAsync(result);
       
        return await unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<CategoryReadDTO>> GetAllCategoryAsync() => (await this._categoryRepository.GetAllCategoryAsync()).Map();
    


    public async Task<OneOf<CategoryReadDTO, Errors>> GetCategoryByIdAsync(string id)
    {
        var  result = (await _categoryRepository.GetCategoryByIdAsync(id));

        if(result is null)
           return Errors.EmiteError("Category not found", nameof(Category));

        return result.Map();


    }

    public async Task<OneOf<CategoryReadDTO, Errors>> UpdateCategoryAsync(CategoryUpdateDTO categoryUpdateDTO, string id)
    {

        if (Errors.TryValid(await this._updateValidator.ValidateAsync(categoryUpdateDTO), out Errors errors))
            return errors;

        var category = await this._categoryRepository.GetCategoryByIdAsync(id);

        if (category is null)
            return Errors.EmiteError("Category not found",nameof(Category));



        category.UpdateName(categoryUpdateDTO.Name);

       await  _unitOfWork.SaveAsync();

        return category.Map();
    }


  
        
    
}
