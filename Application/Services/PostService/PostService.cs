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
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PostService;

public class PostService (
    IPostRepository postRepository,
     IValidator<PostCreateDTO> validatorCreate,
     IValidator<PostUpdateDTO> validatorUpdate
    ) :IPostService
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IValidator<PostCreateDTO> _validatorCreate = validatorCreate;
    private readonly IValidator<PostUpdateDTO> _validatorUpdate = validatorUpdate;


    public async Task<OneOf<PostReadDTO, Errors>> Create(PostCreateDTO addPostInputModel)
    {


        ValidationResult result =  await this._validatorCreate.ValidateAsync(addPostInputModel);
        if (!result.IsValid)
                    return  Errors.Factory.CreateErro( result.Errors.Select( x => new AppErro(x.ErrorMessage, x.PropertyName) ) );


        var post = await _postRepository.Create((Post)addPostInputModel);
        return post.Map();
    }

    public async Task<OneOf<bool, Errors>> DeleteById(string id)
    {
        var result = await _postRepository.DeleteById(id);

        if (!result )
            return Errors.Factory.CreateErro(errors: [new AppErro("post not found", nameof(Post))]);

        return result;
    }

    public async Task<List<PostReadDTO>> GetAll()
    {
        return (await _postRepository.GetAllPosts()).Map();
    }

    public async Task<OneOf<PostReadDTO, Errors>> GetById(string id)
    {

        var result = await _postRepository.GetById(id);

        if (result is null) 
            return Errors.Factory.CreateErro(errors: [new AppErro("post not found", nameof(Post))]);


        return result.Map();
    }

    public async Task<OneOf<PostReadDTO, Errors>> Update(PostUpdateDTO post, string id)
    {
        ValidationResult resultValidation = await this._validatorUpdate.ValidateAsync(post);

        if (!resultValidation.IsValid)
            return Errors.Factory.CreateErro(
                resultValidation.Errors.Select(x =>
 
                   new AppErro(x.ErrorMessage, x.PropertyName)
            )   );


       var postUpdate = await _postRepository.GetById(id);


        if (postUpdate is null)
            return Errors.Factory.CreateErro(errors: [new AppErro("post not found", nameof(Post))]);

        postUpdate.UpdateAttributes(post.Title, post.Text);

        await _postRepository.Save();

        return postUpdate.Map();



    }
}
