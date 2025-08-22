using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository.IAuthorRepository;

public interface IAuthorRepository
{
    Task<List<Author>> GetAllAsync();

    Task<Author> GetById(string id);

    Task<bool> Create(Author author);

    Task<Author> Update(Author author, string id);

    Task<bool> DeleteById(string id);
}
