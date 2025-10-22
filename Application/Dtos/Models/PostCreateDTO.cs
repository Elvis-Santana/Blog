using Application.Dtos.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Models;

public record PostCreateDTO(string Title, string Text, DateTime Date, string? CategoryId, string AuthorId)
{


    public static explicit operator Post(PostCreateDTO addPostInputModel)
        => Post.Factory.CreatePost(addPostInputModel.Title,
            addPostInputModel.Text,
            addPostInputModel.Date,
            addPostInputModel.CategoryId ?? null,
            addPostInputModel.AuthorId);
    



}


