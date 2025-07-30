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

    Task<Author> GetById(Guid id);

    Task<bool> Create(Author author);

    Task<bool> Delete(Guid id);
}
