using Application.Dtos.Models;
using Application.IServices;
using Domain.Erros;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace TESTE__UNIARIO.Endpoints;

public static class AuthorEndpoints
{
    public static WebApplication RouterAuthorEndpoints(this WebApplication app)
    {
        const string nameGroup = "authors";
        var authors = app.MapGroup($"/{nameGroup}");

        authors.WithTags(nameGroup);


        authors.MapPost("/", async ( AddAuthorInputModel author, [FromServices]IServiceAuthor authorService) =>
        {
            var result = await authorService.CreateAuthor(author);

            return  result.Match<IResult>(
                sucesso => Results.Created($"{nameGroup}/{sucesso.Id}",sucesso),
                erro => Results.BadRequest(erro)
            );

        });

        authors.MapGet("/", async (IServiceAuthor authorService) =>
        {
            return Results.Ok(await authorService.GetAuthor());
        });

        authors.MapGet("/{id}", async (IServiceAuthor authorService,string id) =>
        {
            var res = await authorService.GetById(id);
            return Results.Ok(res.AsT0);
        });

        authors.MapDelete("/{id}", async (IServiceAuthor authorService, string id) =>
        {
            var res = await authorService.DeleteById(id);
            return Results.Ok(res.AsT0);
        });

        authors.MapPatch("/{id}", async (IServiceAuthor authorService, AddAuthorInputModel addAuthorInputModel,string id) =>
        {
            var res = await authorService.Update(addAuthorInputModel,id);
            return Results.Ok(res.AsT0);
        });



        return app;
    }

   
}
