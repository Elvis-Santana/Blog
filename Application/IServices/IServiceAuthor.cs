using Application.Dtos.AuthorViewModel;
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
    Task<OneOf<bool,Errors>> CreateAuthor(AddAuthorInputModel author);
    Task<List<AuthorViewModel>> GetAuthor();

}
