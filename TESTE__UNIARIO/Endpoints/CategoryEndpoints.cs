using Application.Dtos.Models;
using Application.IServices;
using Application.Services.CategoryService;
using Microsoft.AspNetCore.Mvc;

namespace TESTE__UNIARIO.Endpoints;

public static class CategoryEndpoints
{
    public static WebApplication RouterCategoryEndpoints(this WebApplication app)
    {

        var categorys = app.MapGroup("/Categorys");

        categorys.WithTags("Categorys");

        categorys.MapGet("", async (ICategoryService _categoryService) =>
        {
            return Results.Ok((await _categoryService.GetAsync()).AsT0);
        });

        categorys.MapPost("", async (AddCategoryInputModel addCategoryInputModel,ICategoryService _categoryService ) =>
        {
           var result =  await _categoryService.Create(addCategoryInputModel);
            return result.Match<IResult>(
                (res) => Results.Ok(res),
                (err) => Results.BadRequest(err)
            );
        });


        categorys.MapGet("/:id", async (string id,ICategoryService _categoryService) =>
        {
            return Results.Ok((await _categoryService.GetById(id)).AsT0);
        });
        categorys.MapDelete("/:id", async (string id,ICategoryService _categoryService) =>
        {
            return Results.Ok((await _categoryService.DeleteById(id)).AsT0);
        });

        categorys.MapPatch("/:id", async (UpdateCategoryInputModel updateCategoryInputModel,string id, ICategoryService _categoryService) =>
        {
            var result = await _categoryService.Update(updateCategoryInputModel,id);

            return result.Match<IResult>(
                 (res) => Results.Ok(res),
                 (err) => Results.NotFound(err)
             );
        });


       

        return app;
    }


}
