

using Application.IRepository.IAuthorRepository;
using Domain.Entities;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class AuthorRepository(DbContextLite context) : IAuthorRepository
{
    private readonly DbContextLite _context =context;

   

    public async Task CreateAuthorAsync(Author author)
    {
        await  this._context.Authors.AddAsync(author);
    }
        

    public  void RemoveAuthor(Author author)
    {
        this._context.Authors.Remove(author);
        
    }

    public async Task<IEnumerable<Author>> GetAllAuthorAsync() => await _context.Authors
       .AsNoTracking()
       .Include(x => x.Followers)
       .Include(x => x.Post)
       .ThenInclude(p => p.Category)
       .ToListAsync();



    public async Task<Author?> GetByExpression(Func<Author, bool> filtro)
    {
        var result = (await _context.Authors.ToListAsync()).FirstOrDefault(filtro);


        return result;
    }
        
       
    

    public async Task<Author?> GetAuthorByIdAsync(string id) 
        => (await this._context.Authors.Include(x => x.Post)
           .ThenInclude(p => p.Category)
           .FirstOrDefaultAsync(x => x.Id.Equals(id)));
    



 

   
}
