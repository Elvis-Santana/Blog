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

public interface IServiceAuthor
{
    Task<OneOf<AuthorViewModel, Errors>> CreateAuthor(AddAuthorInputModel author);
    Task<List<AuthorViewModel>> GetAuthor();


    Task<OneOf<AuthorViewModel, Errors>> GetById(string id);

    Task<OneOf<AuthorViewModel, Errors>> Update(AddAuthorInputModel author, string id);

    Task<OneOf<bool, Errors>> DeleteById(string id);


}
