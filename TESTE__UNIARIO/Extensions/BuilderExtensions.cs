using Application;
using Application.Dtos.Models;
using Application.IServices;
using Application.Services.AuthorService;
using Application.Services.AuthSevice;
using Application.Services.CategoryService;
using Application.Services.PostService;
using Application.Validators.Validator.AuthorValidator;
using Application.Validators.Validator.CategoryValidator;
using Application.Validators.Validator.PostValidator;
using Domain.IRepository.IAuthorRepository;
using Domain.IRepository.ICategoryRepository;
using Domain.IRepository.IPostRepository;
using FluentValidation;
using Infrastructure.Db;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace TESTE__UNIARIO.Extensions;

public static class BuilderExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services,IConfiguration builder)
    {
        services.AddDbContext<DbContextLite>(options => options.UseSqlite(builder.GetConnectionString("DefaultConnection")));
        //services.AddDbContext<DbContextLite>(options => options.UseSqlServer("postgresql://postgres:12345@localhost:5432/postgres"));


        ScopedRepository(services);
        ScopedService(services);
        Validator(services);
        ConfigSwagger(services);

        return services;

        static void Validator(IServiceCollection services)
        {
            services.AddScoped<IValidator<AuthorCreateDTO>, AuthorCreateValidator>();
            services.AddScoped<IValidator<CategoryCreateDTO>, CategoryCreateValidator>();
            services.AddScoped<IValidator<CategoryUpdateDTO>, CategoryUpdateValidator>();
            services.AddScoped<IValidator<PostCreateDTO>, PostCreateValidator>();
            services.AddScoped<IValidator<PostUpdateDTO>, PostUpdateValidator>();
        }

        static void ScopedService(IServiceCollection services)
        {
            services.AddScoped<IAuthSevice, AuthSevice>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IServiceAuthor, AuthorService>();
            services.AddScoped<IPostService, PostService>();
        }

        static void ScopedRepository(IServiceCollection services)
        {
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }

        static void ConfigSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
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


}
