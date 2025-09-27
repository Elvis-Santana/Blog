using Application.Dtos.Models;
using Application.IServices;
using Domain.Entities;
using Domain.Erros;
using Domain.Erros.AppErro;
using Domain.IRepository.IPostRepository;
using FluentValidation;
using FluentValidation.Results;
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

    public async Task<OneOf<PostViewModel, Errors>> Create(AddPostInputModel addPostInputModel)
    {


        ValidationResult result =  await this._validator.ValidateAsync(addPostInputModel);
        if (!result.IsValid)
                    return  Errors.Factory.CreateErro( result.Errors.Select( x => new AppErro(x.ErrorMessage, x.PropertyName) ) );


        var post = await _postRepository.Create((Post)addPostInputModel);
        return post.Map();
    }

    public async Task<List<PostViewModel>> GetAll()
    {
        return (await _postRepository.GetAllPosts()).Map();
    }
}
