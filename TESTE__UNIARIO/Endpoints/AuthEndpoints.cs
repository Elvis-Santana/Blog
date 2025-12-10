using Application.Dtos.Models;
using Application.IServices;
using Application.Services.AuthSevice;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace TESTE__UNIARIO.Endpoints;

public static class AuthEndpoints
{

    public static WebApplication RouterAuthEndpoints(this WebApplication app)
    {
        const string nameGroup = "auth";
        var auth = app.MapGroup($"/{nameGroup}");

        auth.WithTags(nameGroup);


        auth.MapPost("/",async (IAuthSevice authSevice, Login login) =>
        {
            var token = await authSevice.CriateToken(login);
            return Results.Ok(token);
        });

        auth.MapGet("/", () => Results.Ok(true)).RequireAuthorization();


        return app;
    }
}
