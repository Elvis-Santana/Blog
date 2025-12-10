using Application.Dtos.Models;
using Domain.Erros;
using OneOf;

namespace Application.IServices;

public interface IPostService
{
    Task<IEnumerable<PostReadDTO>> GetAllPostsAsync();


    Task<OneOf<PostReadDTO, Errors>> CreatePostAsync(PostCreateDTO postCreateDTO);

    Task<OneOf<PostReadDTO, Errors>> GetPostByIdAsync(string id);

    Task<OneOf<bool, Errors>> RemovePostByIdAsync(string id);

    Task<OneOf<PostReadDTO, Errors>> UpdatePostAsync(PostUpdateDTO post, string id);
}
