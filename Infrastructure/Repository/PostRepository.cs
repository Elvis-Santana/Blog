using Domain.Entities;
using Domain.IRepository.IPostRepository;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository;

public class PostRepository (DbContextLite dbContext) : IPostRepository
{
    private readonly DbContextLite _dbContext = dbContext;

    public async Task<bool> Create(Post post)
    {
        await _dbContext.Posts.AddAsync(post);

        return await _dbContext.SaveChangesAsync() >0;
    }

    public Task<Post> DeleteById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Post>> GetAllPosts()=> await _dbContext.Posts
        .Include(c =>c.Category)
        .Include(x => x.Author)
        
        .ToListAsync();

    public Task<Post> GetById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Post> Update(Post category, string id)
    {
        throw new NotImplementedException();
    }
}
