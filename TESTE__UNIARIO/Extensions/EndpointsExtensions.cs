using Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace TESTE__UNIARIO.Extensions;

public static class EndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {

       app.RouterAuthorEndpoints();
       app.RouterPostEndpoints();
       app.RouterCategoryEndpoints();

        //app.MapPost("/login", async (string senha, IAuthSevice _authSevice) =>
        //{

        //    return Results.Ok(await _authSevice.CriateToken(senha));
        //});
        return app;
    }



}
