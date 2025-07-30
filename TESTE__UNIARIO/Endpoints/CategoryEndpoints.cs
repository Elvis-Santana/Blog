using Application.Dtos.Models;
using Application.IServices;

namespace TESTE__UNIARIO.Endpoints;

public static class CategoryEndpoints
{
    public static WebApplication RouterCategoryEndpoints(this WebApplication app)
    {

        var author = app.MapGroup("/Category");

        author.MapPost("", async (AddCategoryInputModel addCategoryInputModel,ICategoryService _categoryService ) =>
        {
           var result =  await _categoryService.Create(addCategoryInputModel);
            return result.Match<IResult>(
                (res) => Results.Ok(res),
                (err) => Results.BadRequest(err)
            );
        });

        author.MapGet("", async (ICategoryService _categoryService) =>
        {
            return (await _categoryService.GetAsync()).AsT0;
        });


        return app;
    }
}
