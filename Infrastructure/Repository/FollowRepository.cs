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
        _dbContextLite.Followers.Add(follow);
    }

    public void RemoveFollow(Follow follow)
    {
        _dbContextLite.Remove(follow);
    }
}
