using Application.IRepository.IAuthorRepository;
using Application.IRepository.ICategoryRepository;
using Application.IRepository.IPostRepository;
using Application.IUnitOfWork;
using Infrastructure.Db;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureConection
{
    public static IServiceCollection AddInfrastructure( this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DbContextLite>(options =>
            options.UseSqlite(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}
