using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository.ICategoryRepository;

public interface ICategoryRepository
{
    Task<Category> Create(Category category);

    Task<List<Category>> GetAsync();

    Task<bool> DeleteById(string id);

    Task<Category> GetById(string id);

    Task<Category> Update(Category category, string id);

}
