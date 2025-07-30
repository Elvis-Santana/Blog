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

    public async Task<bool> Create(Category category)
    {

        await  this._dbContextLite.Category.AddAsync(category);

        return await this._dbContextLite.SaveChangesAsync() > 0;
    }

    public async Task<List<Category>> GetAsync()=> await _dbContextLite.Category.ToListAsync(); 
    
}
