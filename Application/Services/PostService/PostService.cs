using Application.Dtos.Models;
using Application.IServices;
using Domain.Entities;
using Domain.Erros;
using Domain.IRepository.IPostRepository;
using Mapster;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.PostService;

public class PostService (IPostRepository postRepository) :IPostService
{
    private readonly IPostRepository _postRepository = postRepository;

    public async Task<OneOf<bool, Errors>> Create(AddPostInputModel addPostInputModel)
    {

        return await _postRepository.Create(addPostInputModel.Adapt<Post>());
    }

    public async Task<List<PostViewModel>> GetAll()
    {
        return (await _postRepository.GetAllPosts()).Adapt<List<PostViewModel>>();
    }
}
