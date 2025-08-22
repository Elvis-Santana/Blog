using Domain.Entities;
using Domain.IRepository.IAuthorRepository;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository;

public class AuthorRepository(DbContextLite context) : IAuthorRepository
{
    private readonly DbContextLite _context =context;

   

    public async Task<bool> Create(Author author)
    {
        await  this._context.Authors.AddAsync(author);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteById(string id)
    {

        var author =  await this.GetById(id);

        this._context.Authors.Remove(author);

          return  await _context.SaveChangesAsync() >0;
    }

    public async Task<List<Author>> GetAllAsync() => await _context.Authors
       .Include(x => x.Post)
       .ThenInclude(p => p.Category)
       .ToListAsync();

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

        authorToUpdate.Name.FirstName = author.Name.FirstName;
        authorToUpdate.Name.LastName = author.Name.LastName;


        await _context.SaveChangesAsync();


        return authorToUpdate;


    }
}
