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

    public async Task<Category> Create(Category category)
    {

        await  this._dbContextLite.Category.AddAsync(category);

         await this._dbContextLite.SaveChangesAsync();
        return category;
    }

    public async Task<bool> DeleteById(string id)
    {
        Category res = await this.GetById(id);

         this._dbContextLite.Category.Remove(res);

        return await this._dbContextLite.SaveChangesAsync() > 0;

    }

    public async Task<List<Category>> GetAsync()
    {
        var result = await _dbContextLite.Category.ToListAsync();
        return result;

    }

    public async Task<Category> GetById(string id)
    {
        var _category = await _dbContextLite.Category.FindAsync(id);

        return _category;

    }


    public async Task<Category> Update(Category category, string id)
    {

         this._dbContextLite.Category.Update(category);

        await this._dbContextLite.SaveChangesAsync();

        return category;


    }
}
