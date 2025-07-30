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

public class AuthorRepository : IAuthorRepository
{
    private readonly DbContextLite _context;

    public AuthorRepository(DbContextLite context)
    {
        _context = context;
    }

    public async Task<bool> Create(Author author)
    {
        await  this._context.Authors.AddAsync(author);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Delete(Guid id)
    {
        await   this._context.Authors.Where(x => x.Id.Equals(id)).ExecuteDeleteAsync();
        return await _context.SaveChangesAsync() > 0;

    }

    public async Task<List<Author>> GetAllAsync() => await _context.Authors.ToListAsync();

    public Task<Author> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
}
