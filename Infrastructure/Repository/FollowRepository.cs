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
    public async Task CreateFollow(Follow follow)
    {
       await _dbContextLite.Followers.AddAsync(follow);
    }

    public void RemoveFollow(Follow follow)
    {
        _dbContextLite.Remove(follow);
    }
}
