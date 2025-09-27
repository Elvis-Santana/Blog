using Application.Dtos.Models;
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
    Task<List<PostViewModel>> GetAll();


    Task<OneOf<PostViewModel, Errors>> Create(AddPostInputModel addPostInputModel);
}
