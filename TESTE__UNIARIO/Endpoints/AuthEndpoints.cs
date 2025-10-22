using Application.Dtos.Models;
using Application.IServices;
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
            return await authSevice.CriateToken(login);
        });


        return app;
    }
}
