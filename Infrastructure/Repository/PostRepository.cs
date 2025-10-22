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

public class PostRepository(DbContextLite dbContext) : IPostRepository
{
    private readonly DbContextLite _dbContext = dbContext;

    public async Task<Post> Create(Post post)
    {
        await _dbContext.Posts.AddAsync(post);

        await this.Save();
        return post;
    }

    public async Task<bool> DeleteById(string id)
    {
        var result = await this.GetById(id);

        if (result is null)
            return false;
        
        this._dbContext.Posts.Remove(result);
        return await this.Save() ;
        
        
    }

    public async Task<List<Post>> GetAllPosts()=> await _dbContext.Posts
        .Include(c =>c.Category)
        .Include(x => x.Author)  
        .ToListAsync();

    public async Task<Post> GetById(string id)=> await _dbContext.Posts.FindAsync(id);

    public async Task<bool> Save() => 
        await this._dbContext.SaveChangesAsync() > 0;
    
     
    

    public async Task<Post> Update(Post post, string id)
    {
        var result = await this.GetById(id);

        if (result is null)
            return result;

        this._dbContext.Posts.Update(post);
        await this.Save();
        return post;
    }
}
