using Application.Dtos.AuthorViewModel;
using Application.IServices;
using Domain.Erros;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace TESTE__UNIARIO.Endpoints;

public static class AuthorEndpoints
{
    public static WebApplication RouterAuthorEndpoints(this WebApplication app)
    {
        var author = app.MapGroup("/author");

        author.MapPost("/", async (AddAuthorInputModel author, IServiceAuthor authorService) =>
        {
            OneOf<bool, Errors> result = await authorService.CreateAuthor(author);

            return  result.Match<IResult>(
                sucesso => Results.Ok(sucesso),
                erro => Results.BadRequest(erro)
            );

        });

        author.MapGet("/", async (IServiceAuthor authorService) =>
        {
            return Results.Ok(await authorService.GetAuthor());
        });


        return app;
    }

   
}
