using Application.Dtos.Models;
using Application.IServices;

namespace TESTE__UNIARIO.Endpoints;

public static class PostEndpoints
{
    public static WebApplication RouterPostEndpoints(this WebApplication app)
    {
        var posts = app.MapGroup("/posts");
        posts.WithTags("posts");


        posts.MapPost("/", async (AddPostInputModel post, IPostService postService) =>
        {
            return (await postService.Create(post)).Match<IResult>(
                (s) => Results.Ok(s),

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
