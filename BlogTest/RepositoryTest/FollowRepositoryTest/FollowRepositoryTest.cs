using Application.Dtos.FollowDTO;
using Application.IRepository.IFollowRepository;
using BlogTest.Scenario;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Db;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogTest.RepositoryTest.FollowRepositoryTest;

public class FollowRepositoryTest
{
    private readonly Faker _faker = new("pt_BR");
    private readonly DbContextLite _dbContextLite;
    private readonly IFollowRepository _repository;

    public FollowRepositoryTest()
    {
        var Options = new DbContextOptionsBuilder<DbContextLite>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
     .Options;

        this._dbContextLite = new(Options);
        this._repository = new FollowRepository(this._dbContextLite);
    }

    [Fact]
    public async Task CreateFollow_QuandoValoresValidosSaoAtribuidos_DeveSerSalvoComSucesso()
    {
         string followerId = Guid.NewGuid().ToString();
         string FollowingId = Guid.NewGuid().ToString();

        Follow follow = Follow.CreateFollow(
            followerId,
            FollowingId
        );

        _repository.CreateFollow( follow );
         bool result =  await _dbContextLite.SaveChangesAsync() > 0;

        result.Should().BeTrue();
    }

    
}
