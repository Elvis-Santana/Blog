using Application.Dtos.Models;
using Domain.Entities;
using Domain.ObjectValues;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Application;

public static class MappingConfigurations
{
    public static void RegisterMap(this IServiceCollection services)
    {
        services.AddMapster();

        TypeAdapterConfig<Author, AuthorViewModel>.NewConfig()
            .ConstructUsing(member => new(member.Id, member.Name, member.Post.Adapt<List<PostViewModel>>() ));

        TypeAdapterConfig<AddAuthorInputModel,Author >.NewConfig()
         .ConstructUsing(dest => new (dest.Name));

        TypeAdapterConfig<AddCategoryInputModel, Category>.NewConfig()
            .ConstructUsing(src => new Category(src.AuthorId, src.Name));


        TypeAdapterConfig<Category, CategoryViewModel>.NewConfig()
            .ConstructUsing(dest => new CategoryViewModel(dest.Id, dest.AuthorId,  dest.Name));

       

        TypeAdapterConfig<Post, PostViewModel>.NewConfig()
            .ConstructUsing(
            dest => 
                new(
                    dest.Id,
                    dest.Title,
                    dest.Text,
                    dest.Date,
                    dest.CategoryId,
                    dest.Category.Adapt<CategoryViewModel>(),
                    dest.AuthorId
                    
                 )
            );
        TypeAdapterConfig<AddPostInputModel, Post>.NewConfig()
                 .ConstructUsing(src =>
                     new(src.title, src.text, src.date, src.categoryId??null, src.authorId)
                 );
        TypeAdapterConfig<UpdateCategoryInputModel, Category>.NewConfig()
              .Map(dest => dest.Name, src => src.Name);




        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());


    }
}
