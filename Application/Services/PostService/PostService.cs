using Application.Dtos.Models;
using Application.IServices;
using Domain.Entities;
using Domain.Erros;
using Domain.Erros.AppErro;
using Domain.IRepository.IPostRepository;
using FluentValidation;
using Mapster;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PostService;

public class PostService (IPostRepository postRepository, IValidator<AddPostInputModel> validator) :IPostService
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IValidator<AddPostInputModel> _validator = validator;

    public async Task<OneOf<bool, Errors>> Create(AddPostInputModel addPostInputModel)
    {

        TypeAdapterConfig<AddPostInputModel, Post>.NewConfig()
      .ConstructUsing(src =>
          new Post(src.title, src.text, src.date, src.categoryId, src.authorId)
      );

        var result =  await this._validator.ValidateAsync(addPostInputModel);

        if (!result.IsValid)
        {
            return new Errors(
                result.Errors.Select(
                    x => new AppErro(x.ErrorMessage, x.PropertyName)
                ).ToList()
             );
        }


        return await _postRepository.Create(addPostInputModel.Adapt<Post>());
    }

    public async Task<List<PostViewModel>> GetAll()
    {
        return (await _postRepository.GetAllPosts()).Adapt<List<PostViewModel>>();
    }
}
