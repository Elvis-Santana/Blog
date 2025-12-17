using Application.Dtos.Models;
using Application.IServices;
using Domain.Entities;
using Domain.Erros;
using Domain.IRepository.IPostRepository;
using FluentValidation;
using Infrastructure.UnitOfWork;
using OneOf;

namespace Application.Services.PostService;

public class PostService (
    IPostRepository postRepository,
     IValidator<PostCreateDTO> validatorCreate,
     IValidator<PostUpdateDTO> validatorUpdate,
     IUnitOfWork unitOfWork
    ) :IPostService
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IValidator<PostCreateDTO> _validatorCreate = validatorCreate;
    private readonly IValidator<PostUpdateDTO> _validatorUpdate = validatorUpdate;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;


    public async Task<OneOf<PostReadDTO, Errors>> CreatePostAsync(PostCreateDTO addPostInputModel)
    {

        if (Errors.TryValid(await this._validatorCreate.ValidateAsync(addPostInputModel), out Errors errors))
            return errors;

        var result = (Post)addPostInputModel;
        await _postRepository.CreatePost(result);
        await _unitOfWork.SaveAsync();

        return result.Map();
    }

    public async Task<OneOf<bool, Errors>> RemovePostByIdAsync(string id)
    {
       var resul = await  this._postRepository.GetPostsById(id);

        if (resul is null)
            return Errors.EmiteError("post not found", nameof(Post));

         _postRepository.RemovePost(resul);

        return await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<PostReadDTO>> GetAllPostsAsync()=> (await _postRepository.GetAllPosts()).Map();
   

    public async Task<OneOf<PostReadDTO, Errors>> GetPostByIdAsync(string id)
    {

        var result = await _postRepository.GetPostsById(id);

        if (result is null)
            return Errors.EmiteError("post not found", nameof(Post));


        return result.Map();
    }

    public async Task<OneOf<PostReadDTO, Errors>> UpdatePostAsync(PostUpdateDTO post, string id)
    {

        if (Errors.TryValid(await this._validatorUpdate.ValidateAsync(post), out Errors errors))
            return errors;


        Post? postUpdate = await _postRepository.GetPostsById(id);


        if (postUpdate is null)
            return Errors.EmiteError("post not found", nameof(Post));

          postUpdate.UpdateAttributes(post.Title, post.Text,post.CategoryId);
        await _unitOfWork.SaveAsync();

        await _postRepository.LoadCategoryReferenceAsync(postUpdate);



        return postUpdate.Map();
    }
}
