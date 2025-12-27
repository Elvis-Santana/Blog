using Application.IRepository.IPostRepository;
using Domain.Entities;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class PostRepository(DbContextLite dbContext) : IPostRepository
{
    private readonly DbContextLite _dbContext = dbContext;

    public async Task CreatePost(Post post) =>  await _dbContext.Posts.AddAsync(post);
    
        
    public void RemovePost(Post post) => this._dbContext.Posts.Remove(post);
    

    public async Task<IEnumerable<Post>> GetAllPosts()=> await _dbContext.Posts
        .AsNoTracking()
        .Include(c =>c.Category)
        .Include(x => x.Author)  
        .ToListAsync();

    public async Task<Post?> GetPostsById(string id) => await _dbContext.Posts
        .Include(a => a.Author)
        .Include(c => c.Category)
        .FirstOrDefaultAsync(p => p.Id.Equals(id));

    public async Task LoadCategoryReferenceAsync(Post post)
    {
        await _dbContext.Entry(post) .Reference(p => p.Category) .LoadAsync();
    }
}
