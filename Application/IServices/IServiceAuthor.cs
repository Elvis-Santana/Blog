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
    Task<OneOf<AuthorReadDTO, Errors>> CreateAuthor(AuthorCreateDTO author);
    Task<List<AuthorReadDTO>> GetAuthor();


    Task<OneOf<AuthorReadDTO, Errors>> GetById(string id);

    Task<OneOf<AuthorReadDTO, Errors>> Update(AuthorCreateDTO author, string id);

    Task<OneOf<bool, Errors>> DeleteById(string id);


}
