using Application.Dtos.Models;
using Application.IServices;

namespace TESTE__UNIARIO.Endpoints;

public static class PostEndpoints
{
    public static WebApplication RouterPostEndpoints(this WebApplication app)
    {
        const string nameGroup = "posts";

        var posts = app.MapGroup($"/{nameGroup}");

        posts.WithTags(nameGroup);


        posts.MapPost("/", async (AddPostInputModel post, IPostService postService) =>
        {
            return (await postService.Create(post)).Match<IResult>(
                (sucess) => Results.Created($"{post}/{sucess.Id}", sucess),

                (erros) => Results.BadRequest(erros)
            );
                
        });

        posts.MapGet("/", async (IPostService postService) =>
        {
            var result = (await postService.GetAll());

            return result;
        });


        return app;
    }
}
