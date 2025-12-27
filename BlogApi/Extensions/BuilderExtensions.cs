using Application;
using Infrastructure;
using Microsoft.OpenApi.Models;

namespace TESTE__UNIARIO.Extensions;

public static class BuilderExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration builder)
    {
        return services
        .AddInfrastructure(builder)
        .AddApplication()
        .AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Minha Minimal API",
                Version = "v1",
                Description = "Documentação automática com Swagger",
                Contact = new OpenApiContact
                {
                    Name = "Elvis",
                    Email = "seuemail@exemplo.com"
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement {

                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{}
                }

            });
        });   
    }


}
