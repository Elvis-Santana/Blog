namespace TESTE__UNIARIO.Extensions;

public static class EndpointsExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app) => app
        .RouterAuthorEndpoints()
        .RouterPostEndpoints()
        .RouterCategoryEndpoints();
    

    
}
