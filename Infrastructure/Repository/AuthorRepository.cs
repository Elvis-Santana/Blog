

using Domain.Entities;
using Domain.IRepository.IAuthorRepository;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class AuthorRepository(DbContextLite context) : IAuthorRepository
{
    private readonly DbContextLite _context =context;

   

    public async Task<Author> Create(Author author)
    {
        await  this._context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        return author;
    }
        

    public async Task<bool> DeleteById(string id)
    {

        var author =  await this.GetById(id);

        if (author is not null)
            this._context.Authors.Remove(author);
        

       return  await _context.SaveChangesAsync() >0;
    }

    public async Task<List<Author>> GetAllAsync() => await _context.Authors
       .Include(x => x.Post)
       .ThenInclude(p => p.Category)
       .ToListAsync();



    public async Task<Author?> GetByExpression(Func<Author, bool> filtro)
    {
        var result = (await _context.Authors.ToListAsync()).FirstOrDefault(filtro);


        return result;
    }
        
       
    

    public async Task<Author> GetById(string id)
    {
        
        return (await this._context.Authors
           .Include(x => x.Post)
           .ThenInclude(p => p.Category)
           .FirstOrDefaultAsync(x => x.Id.Equals(id)))!;
    }



    public async Task<Author> Update(Author author, string id)
    {
        var authorToUpdate = await this.GetById(id);

        if (authorToUpdate is not null)
        {
            authorToUpdate.Name.FirstName = author.Name.FirstName;
            authorToUpdate.Name.LastName = author.Name.LastName;


            await _context.SaveChangesAsync();
        }


        return authorToUpdate;


    }

   
}
