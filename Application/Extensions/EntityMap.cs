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
    public static AuthorViewModel Map(this Author author) => 
         new (
                author.Id,
                author.Name,
                author.Post.Count.Equals(0) 
                ? new List<PostViewModel>()
                : author.Post.Select(a => a.Map()).ToList()
         );


    public static List<AuthorViewModel> Map(this IEnumerable<Author> author) => author.Select(a => a.Map()).ToList();


    public static PostViewModel Map(this Post post) =>  
        new (
             post.Id, 
             post.Title,
             post.Text, 
             post.Date,
             post.CategoryId ??null ,
             post.Category is null ?null:post.Category.Map(),
             post.AuthorId
        );

    public static List<PostViewModel> Map(this IEnumerable<Post> post) => post.Select(a => a.Map()).ToList();



    public static CategoryViewModel Map(this Category category)=>
        new (
            category.Id,
            category.AuthorId,
            category.Name
        );

    public static List<CategoryViewModel> Map(this IEnumerable<Category> category) => category.Select(a => a.Map()).ToList();



}
