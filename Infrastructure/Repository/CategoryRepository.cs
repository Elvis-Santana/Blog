using Domain.Entities;
using Domain.IRepository;
using Domain.IRepository.ICategoryRepository;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly DbContextLite _dbContextLite;

    public CategoryRepository(DbContextLite dbContextLite)
    {
        _dbContextLite = dbContextLite;
    }

    public async Task CreateCategoryAsync(Category category)
    {
        await  this._dbContextLite.Category.AddAsync(category);
    }

    public void RemoveCategoryAsync(Category category)
    {
      
       this._dbContextLite.Category.Remove(category);

    }

    public async Task<IEnumerable<Category>> GetAllCategoryAsync() =>
      await _dbContextLite.Category.ToListAsync();
        

    
    public async Task<Category?> GetCategoryByIdAsync(string id)=>
      await _dbContextLite.Category.FindAsync(id);
     

    

    //public async Task<Category> Update(Category category, string id)
    //{

    //     this._dbContextLite.Category.Update(category);

    //    await this._dbContextLite.SaveChangesAsync();

    //    return category;


    //}
}
