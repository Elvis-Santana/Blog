using Application.Dtos.FollowDTO;
using Application.Dtos.Models;
using Domain.Entities;

namespace Application.Extensions;



public static class EntityMapper
{


    public static AuthorReadDTO Map(this Author author) =>  new(
                author.Id,
                author.Name,
                author.Post.Select(a => a.Map()).ToList() ?? new (),
                author.Email,
                author.Followers.Select(a => a.Map()).ToList() ?? new()


    );




    public static PostReadDTO Map(this Post post) =>   new (
             post.Id, 
             post.Title,
             post.Text, 
             post.Date,
             post.CategoryId ,
             post.Category?.Map(),
             post.AuthorId
    );



    public static FollowReadDTO Map(this Follow follow) => new(follow.FollowerId, follow.FollowingId);

    public static CategoryReadDTO Map(this Category category)=> new ( category.Id, category.AuthorId,  category.Name );
    public static IEnumerable<PostReadDTO> Map(this IEnumerable<Post> post) => post.Select(a => a.Map());

    public static IEnumerable<AuthorReadDTO> Map(this IEnumerable<Author> author) => author.Select(a => a.Map());

    public static IEnumerable<CategoryReadDTO> Map(this IEnumerable<Category> category) => category.Select(a => a.Map());



}
