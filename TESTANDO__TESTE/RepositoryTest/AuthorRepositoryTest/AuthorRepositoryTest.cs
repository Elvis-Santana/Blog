using Application.Dtos.AuthorViewModel;
using Bogus;
using Domain.Entities;
using Domain.IRepository.IAuthorRepository;
using Domain.ObjectValues;
using FluentAssertions;
using Infrastructure.Db;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTANDO__TESTE.RepositoryTest.AuthorRepositoryTest;


public class AuthorRepositoryTest
{
    private readonly DbContextOptions<DbContextLite> _dbContextOptions;
    private readonly Faker _faker = new ("pt_BR");

    public AuthorRepositoryTest()
    {


        this._dbContextOptions  = new DbContextOptionsBuilder<DbContextLite>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;


    }

    [Fact]
    public async  Task GetAllAsync_ShouldReturnListAuthors()
    {

        //arrange
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);
        var expecteAuthorId1 = Guid.NewGuid();
        var expecteAuthorId2 = Guid.NewGuid();

        var category = new Category(expecteAuthorId1,"name");

        var expectedPost1 = new Post( this._faker.Person.FirstName, this._faker.Lorem.ToString()!, new DateTime(), category.Id,  expecteAuthorId1);
        var expectedPost2 = new Post( this._faker.Person.FirstName, this._faker.Lorem.ToString()!, new DateTime(), category.Id,  expecteAuthorId2);

        List<Author> expectedAuthoes = new List<Author>()
        {
            new Author(expecteAuthorId1,expectedName, new List<Post>(){expectedPost1}),
            new Author(expecteAuthorId2,expectedName, new List<Post>(){expectedPost2})

        };

        using var dbContext = new DbContextLite(this._dbContextOptions);



        await dbContext.Authors.AddRangeAsync(expectedAuthoes);
        await dbContext.SaveChangesAsync();

        //act
        AuthorRepository authorRepository = new(dbContext);
        var result = await authorRepository.GetAllAsync();

        //assert
        result.Count.Should().Be(2);





    }



    [Fact]
    public async Task Create_ShouldSaveAuthorToDatabase()
    {
        //arrange
        var expectedId = Guid.NewGuid();
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);
        var expecteAuthorId1 = Guid.NewGuid();

        var category = new Category(expecteAuthorId1, "name");
        List<Post> expectedIdPosts = new List<Post>
        {
                 new Post( this._faker.Person.FirstName, this._faker.Lorem.ToString()!, new DateTime(), category.Id,  expecteAuthorId1), 
        };

        //act
        Author author = new(expectedId, expectedName, expectedIdPosts);

        using var dbContext = new DbContextLite(this._dbContextOptions);



        AuthorRepository authorRepository = new(dbContext);
        var result = await authorRepository.Create(author);

        //assert
        result.Should().BeTrue("Author should be created successfully");

    }


}
