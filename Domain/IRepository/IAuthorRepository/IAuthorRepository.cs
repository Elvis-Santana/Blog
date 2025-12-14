using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository.IAuthorRepository;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAuthorAsync();

    Task<Author?> GetAuthorByIdAsync(string id);

    Task<Author?> GetByExpression(Func<Author, bool> expression);

    Task CreateAuthorAsync(Author author);


    void RemoveAuthor(Author author);
}
