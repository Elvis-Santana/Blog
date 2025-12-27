using Application.IUnitOfWork;
using Infrastructure.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork (DbContextLite context) : IUnitOfWork
    {
        private readonly DbContextLite _context = context;
        public async Task<bool> SaveAsync()=>  await _context.SaveChangesAsync() > 0;
        
    }
}
