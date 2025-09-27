using Application;
using Application.Dtos.Models;
using Application.IServices;
using Application.Services.AuthorService;
using Application.Services.CategoryService;
using Application.Services.PostService;
using Domain.IRepository.IAuthorRepository;
using Domain.IRepository.ICategoryRepository;
using Domain.IRepository.IPostRepository;
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


        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IServiceAuthor, AuthorService>();
        services.AddScoped<IPostService, PostService>();


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
        });


        return services;
    }
    

}
