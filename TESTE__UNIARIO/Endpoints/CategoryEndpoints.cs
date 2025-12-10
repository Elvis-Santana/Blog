using Application.Dtos.Models;
using Application.IServices;
using Application.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace TESTE__UNIARIO.Endpoints;

public static class CategoryEndpoints
{
    public static WebApplication RouterCategoryEndpoints(this WebApplication app)
    {

        const string nameGroup = "Categorys";
        var categorys = app.MapGroup($"/{nameGroup}");

        categorys.WithTags(nameGroup);

        categorys.MapGet("/", async (ICategoryService _categoryService) =>
        {
            return Results.Ok((await _categoryService.GetAllCategoryAsync()));
        });

        categorys.MapPost("/", async (CategoryCreateDTO addCategoryInputModel,ICategoryService _categoryService ) =>
        {
           var res =  await _categoryService.CreateCategory(addCategoryInputModel);
            return res.Match(
                res => Results.Created($"{nameGroup}/{res.Id}", res),
                err => Results.BadRequest(err)
            );
        });


        categorys.MapGet("/{id}", async (string id,ICategoryService _categoryService) =>
        {
            var res = await _categoryService.GetCategoryByIdAsync(id);

            return res.Match( res => Results.Ok(res), err => Results.NotFound(err));
        });

        categorys.MapDelete("/{id}", async (string id, ICategoryService _categoryService) =>
        {
            var res = await _categoryService.RemoveCategoryByIdAsync(id);

            return res.Match(  res => Results.Ok(res), err => Results.NotFound(err) );

        });

        categorys.MapPatch("/{id}", async (CategoryUpdateDTO updateCategoryInputModel,string id, ICategoryService _categoryService) =>
        {
            var res = await _categoryService.UpdateCategoryAsync(updateCategoryInputModel,id);

            return res.Match( res => Results.Ok(res), err => Results.NotFound(err));
        });


      
        return app;
    }


}
