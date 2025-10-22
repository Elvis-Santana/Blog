using Application.Dtos.Models;
using Domain.Entities;
using Domain.Erros;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices;

public interface IPostService
{
    Task<List<PostReadDTO>> GetAll();


    Task<OneOf<PostReadDTO, Errors>> Create(PostCreateDTO addPostInputModel);

    Task<OneOf<PostReadDTO, Errors>> GetById(string id);

    Task<OneOf<bool, Errors>> DeleteById(string id);

    Task<OneOf<PostReadDTO, Errors>> Update(PostUpdateDTO post, string id);
}
