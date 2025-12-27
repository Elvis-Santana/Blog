using Application.IRepository.IFollowRepository;
using Domain.Entities;
using Infrastructure.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository;

public class FollowRepository (DbContextLite dbContextLite) : IFollowRepository
{
    private readonly DbContextLite _dbContextLite = dbContextLite;
    public void CreateFollow(Follow follow)
    {
        throw new NotImplementedException();
    }

    public void RemoveFollow(Follow follow)
    {
        throw new NotImplementedException();
    }
}
