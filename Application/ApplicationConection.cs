using Application.Dtos.Models;
using Application.IServices;
using Application.Services.AuthorService;
using Application.Services.AuthSevice;
using Application.Services.CategoryService;
using Application.Services.PostService;
using Application.Validators.Validator.AuthorValidator;
using Application.Validators.Validator.CategoryValidator;
using Application.Validators.Validator.PostValidator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application;

public static class ApplicationConection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
        .AddScoped<IAuthSevice, AuthSevice>()
        .AddScoped<ICategoryService, CategoryService>()
        .AddScoped<IServiceAuthor, AuthorService>()
        .AddScoped<IPostService, PostService>() 
        .AddScoped<IValidator<AuthorCreateDTO>, AuthorCreateValidator>()
        .AddScoped<IValidator<CategoryCreateDTO>, CategoryCreateValidator>()
        .AddScoped<IValidator<CategoryUpdateDTO>, CategoryUpdateValidator>()
        .AddScoped<IValidator<PostCreateDTO>, PostCreateValidator>()
        .AddScoped<IValidator<PostUpdateDTO>, PostUpdateValidator>();

    }
}
