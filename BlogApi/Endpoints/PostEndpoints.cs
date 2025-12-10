using Application.Dtos.Models;
using Application.IServices;
using Application.Services.PostService;
using OneOf.Types;

namespace TESTE__UNIARIO.Endpoints;

public static class PostEndpoints
{
    public static WebApplication RouterPostEndpoints(this WebApplication app)
    {
        const string NAME_GROUP = "posts";
        var posts = app.MapGroup($"/{NAME_GROUP}");
        posts.WithTags(NAME_GROUP);


        posts.MapPost("/", async (PostCreateDTO post, IPostService postService) =>
        {
            return (await postService.CreatePostAsync(post)).Match(
                sucess => Results.Created($"{post}/{sucess.Id}", sucess),
                erros => Results.BadRequest(erros)
            );
                
        });

        posts.MapGet("/", async (IPostService postService) =>
        {
            var res = (await postService.GetAllPostsAsync());

            return Results.Ok(res);
        });

        posts.MapGet("/{id}", async (IPostService postService ,string id) =>
        {
            return (await postService.GetPostByIdAsync(id)).Match(
                sucess => Results.Ok(sucess),
                erros => Results.NotFound(erros)
            );
        });

        posts.MapDelete("/{id}", async (IPostService postService ,string id) =>
        {
            return (await postService.RemovePostByIdAsync(id)).Match(
                sucess => Results.Ok(sucess),
                erros => Results.NotFound(erros)
            );
        });

        posts.MapPatch("/{id}", async (IPostService postService, string id,PostUpdateDTO postUpdateDTO) =>
        {
            var res = await postService.UpdatePostAsync(postUpdateDTO, id);
            return res.Match(
                sucess => Results.Ok(sucess),
                erros => Results.NotFound(erros)
            );
        });



        return app;
    }
}
