using Application.Dtos.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions;

public static class EntityMap
{
    public static AuthorReadDTO Map(this Author author) => 
         new (
                author.Id,
                author.Name,
                author.Post.Count.Equals(0) 
                ? new List<PostReadDTO>()
                : author.Post.Select(a => a.Map()).ToList()
         );


    public static List<AuthorReadDTO> Map(this IEnumerable<Author> author) => author.Select(a => a.Map()).ToList();


    public static PostReadDTO Map(this Post post) =>  
        new (
             post.Id, 
             post.Title,
             post.Text, 
             post.Date,
             post.CategoryId ??null ,
             post.Category is null ?null:post.Category.Map(),
             post.AuthorId
        );

    public static List<PostReadDTO> Map(this IEnumerable<Post> post) => post.Select(a => a.Map()).ToList();



    public static CategoryReadDTO Map(this Category category)=>
        new (
            category.Id,
            category.AuthorId,
            category.Name
        );

    public static List<CategoryReadDTO> Map(this IEnumerable<Category> category) => category.Select(a => a.Map()).ToList();



}
