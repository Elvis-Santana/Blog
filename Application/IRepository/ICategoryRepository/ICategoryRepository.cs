using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository.ICategoryRepository;

public interface ICategoryRepository
{
    Task CreateCategoryAsync(Category category);

    Task<IEnumerable<Category>> GetAllCategoryAsync();

    void RemoveCategoryAsync(Category category);

    Task<Category?> GetCategoryByIdAsync(string id);

    //Task<Category> Update(Category category, string id);

}
