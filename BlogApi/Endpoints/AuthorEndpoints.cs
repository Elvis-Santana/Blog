using Application.Dtos.Models;
using Application.IServices;
using Domain.Erros;
using Microsoft.AspNetCore.Authorization;
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

        authors.MapPost("/", async ( AuthorCreateDTO author, [FromServices]IServiceAuthor authorService) =>
        {
            var res = await authorService.CreateAuthorAsync(author);

            return  res.Match(
                sucesso => Results.Created($"{nameGroup}/{sucesso.Id}",sucesso),
                erro => Results.BadRequest(erro)
            );

        });

        authors.MapGet("/", async (IServiceAuthor authorService) =>
        {
            return Results.Ok(await authorService.GetAllAuthorAsync());

        });

        authors.MapGet("/{id}", async (IServiceAuthor authorService,string id) =>
        {
            var res = await authorService.GetAuthorByIdAsync(id);
            return res.Match( sucesso => Results.Ok(sucesso), erro => Results.NotFound(erro));
        });

        authors.MapDelete("/{id}", async (IServiceAuthor authorService, string id) =>
        {
            var res = await authorService.RemoveAuthorByIdAsync(id);
            return  res.Match( sucesso => Results.Ok(sucesso), erro => Results.NotFound(erro));
        });

        authors.MapPatch("/{id}", async (IServiceAuthor authorService, AuthorCreateDTO addAuthorInputModel,string id) =>
        {
            var res = await authorService.UpdateAuthorAsync(addAuthorInputModel,id);
            return res.Match(sucesso => Results.Ok(sucesso), erro => Results.NotFound(erro));
        });

        return app;
    }
  
}
