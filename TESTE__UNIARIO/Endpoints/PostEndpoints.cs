using Application.Dtos.AuthorViewModel;
using Application.Dtos.Models;
using Application.IServices;

namespace TESTE__UNIARIO.Endpoints;

public static class PostEndpoints
{
    public static WebApplication RouterPostEndpoints(this WebApplication app)
    {
        var author = app.MapGroup("/post");

        author.MapPost("/", async (AddPostInputModel post, IServiceAuthor createAuthorUseCase) =>
        {
            //return Results.Ok(await createAuthorUseCase.CreateAuthor(author));
        });

        author.MapGet("/", async (IServiceAuthor getAuthorUserCase) =>
        {
            //return Results.Ok(await getAuthorUserCase.GetAuthor());
        });


        return app;
    }
}
