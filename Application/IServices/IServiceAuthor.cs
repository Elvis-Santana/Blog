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
    Task<OneOf<AuthorReadDTO, Errors>> CreateAuthorAsync(AuthorCreateDTO author);
    Task<IEnumerable<AuthorReadDTO>> GetAllAuthorAsync();


    Task<OneOf<AuthorReadDTO, Errors>> GetAuthorByIdAsync(string id);

    Task<OneOf<AuthorReadDTO, Errors>> UpdateAuthorAsync(AuthorCreateDTO author, string id);

    Task<OneOf<bool, Errors>> RemoveAuthorByIdAsync(string id);


}
