using Application.IRepository.IAuthorRepository;
using BlogTest.Scenario;
using Bogus;
using Domain.Entities;
using Domain.ObjectValues;
using FluentAssertions;
using Infrastructure.Db;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TESTANDO__TESTE.RepositoryTest.AuthorRepositoryTest;


public class AuthorRepositoryTest
{
    private readonly Faker _faker = new ("pt_BR");
    private readonly  DbContextLite _dbContextLite;
    private readonly IAuthorRepository _repository;

    public AuthorRepositoryTest()
    {

       var Options = new DbContextOptionsBuilder<DbContextLite>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

        _dbContextLite = new DbContextLite(Options);
        _repository = new AuthorRepository(_dbContextLite);

    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListAuthors()
    {
        //arrang



        List<Author> authors = [
            AuthorScenario.CreateAuthor(),
            AuthorScenario.CreateAuthor()

         ];




        await _dbContextLite.Authors.AddRangeAsync(authors);
     
        await _dbContextLite.SaveChangesAsync();

        //act
        var result = (await _repository.GetAllAuthorAsync()).ToList(); ;

        //assert
        result.Count.Should().Be(2);

    }


    [Fact]
    public async Task Create_ShouldSaveAuthorToDatabase()
    {
        //arrange
        Author author =  AuthorScenario.CreateAuthor();


        //act



        await _repository.CreateAuthorAsync(author);
        var result = author;

        //assert
        result.Id.Should().Be(author.Id);
        result.Name.Should().Be(author.Name);
        result.Post.Should().HaveCount(author.Post.Count());

    }


    [Fact] 
    public async Task GetById_ShouldReturnAuthors()
    {
        //ararnge
        var expectedNameList = new List<FullName>()
        {
            new(this._faker.Person.FirstName, this._faker.Person.LastName),
            new(this._faker.Person.FirstName, this._faker.Person.LastName)
        };


        var expecteAuthorId1 = Guid.NewGuid().ToString();
        var expecteAuthorId2 = Guid.NewGuid().ToString();

        var category = Category.Factory.CreateCategory(expecteAuthorId1, this._faker.Person.FirstName);

        var expectedAuthoes = new List<Author>()
        {
            new Author(expecteAuthorId1,expectedNameList[0], Guid.NewGuid().ToString(), _faker.Person.Email),
            new Author(expecteAuthorId2,expectedNameList[1], Guid.NewGuid().ToString(), _faker.Person.Email )
        };


        await _dbContextLite.Authors.AddRangeAsync(expectedAuthoes);
        await _dbContextLite.SaveChangesAsync();

        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.SaveChangesAsync();

        var expectedPost1 =
            Post.Factory.CreatePost(this._faker.Person.FirstName, this._faker.Lorem.ToString()!, new DateTime(), category.Id, expecteAuthorId1);
        var expectedPost2 =
            Post.Factory.CreatePost(this._faker.Person.FirstName, this._faker.Lorem.ToString()!, new DateTime(), category.Id, expecteAuthorId2);

        var expectedPost = new List<Post>() { expectedPost1, expectedPost2 };

        await _dbContextLite.Posts.AddRangeAsync(expectedPost);
        await _dbContextLite.SaveChangesAsync();


        //act

         
        Author result = await _repository.GetAuthorByIdAsync(expectedAuthoes.ToArray()[0].Id);


        //assert

        result.Id.Should().Be(expectedAuthoes[0].Id);
        result.Post[0].Should().BeEquivalentTo(expectedPost.ToArray()[0]);
        result.Name.Should().Be(expectedAuthoes[0].Name);
    }



    [Fact]
    public async Task GetById_ShouldReturnNull()
    {
        //arrange
        Author author = AuthorScenario.CreateAuthor();

        _dbContextLite.Authors.AddRange(author);

        var idError = Guid.NewGuid().ToString();

        //act

        Author result = await _repository.GetAuthorByIdAsync(idError);

        //assert  

        result.Should().BeNull();
    }


    [Fact]
    public async Task GetByExpression_ShouldExpressionNull()
    {
        //arrange
        Author author = AuthorScenario.CreateAuthor();
        _dbContextLite.Authors.AddRange(author);
        var idError = Guid.NewGuid().ToString();

        //act
        Author? result = await _repository.GetByExpression((a => a.Id.Equals(idError) ));

        //assert  
        result.Should().BeNull();
    }
    [Fact]
    public async Task GetByExpression_ShouldExpressionAuthor()
    {
        //arrange
        Author author = AuthorScenario.CreateAuthor();
        _dbContextLite.Authors.AddRange(author);


        //act
        Author? result = await _repository.GetByExpression((a => a.Id.Equals(author.Id)));

        //assert  
        result.Should().BeNull();
    }

    [Fact]
    public async Task Update_ShouldAuthorAtualizado()
    {
        //ararnge
     

        Author expectedAuthor = AuthorScenario.CreateAuthor();


        FullName expectedUpdatedName = new(
            this._faker.Person.FirstName,
            this._faker.Person.LastName
        );

        Author expectedUpdateAuthor = new (
            expectedAuthor.Id, 
            expectedUpdatedName,
            Guid.NewGuid().ToString(),
            _faker.Person.Email
        );


        await _dbContextLite.Authors.AddAsync(expectedAuthor);
        await _dbContextLite.SaveChangesAsync();
        //act

        var IUnitOfWork = new UnitOfWork(_dbContextLite);
        expectedAuthor.UpdateName(expectedUpdatedName);

        await IUnitOfWork.SaveAsync();

        var result = await _dbContextLite.Authors.FindAsync(expectedAuthor.Id);

        //assert
        result!.Id.Should()
            .NotBeEmpty();

        result!.Id.Should()
            .Be(expectedUpdateAuthor.Id);

        result!.Name.Should()
            .BeEquivalentTo(expectedUpdateAuthor.Name);
    }

    [Fact]
    public async Task Update_ShouldAuthorNaoAtualizado()
    {
        //ararnge

        Author expectedAuthor = AuthorScenario.CreateAuthor();

        FullName expectedUpdatedName = new(
            this._faker.Person.FirstName,
            this._faker.Person.LastName
          );

        Author expectedUpdateAuthor = new (
            expectedAuthor.Id,
            expectedUpdatedName,
            Guid.NewGuid().ToString(),
            _faker.Person.Email
          );

        var idError = Guid.NewGuid().ToString();

        await _dbContextLite.Authors.AddAsync(expectedAuthor);
        await _dbContextLite.SaveChangesAsync();

        //act

        var author = await _repository.GetAuthorByIdAsync(idError);

        var IUnitOfWork = new UnitOfWork(_dbContextLite);

        if (author is not null)
            author.UpdateName(expectedUpdateAuthor.Name);

        var result = await IUnitOfWork.SaveAsync();
        //assert

        result.Should().BeFalse();
    }


    [Fact]
    public async Task Delete_ShouldRetrunTrue()
    {
        //ararnge
        Author expectedAuthor = AuthorScenario.CreateAuthor();
        

        await _dbContextLite.Authors.AddAsync(expectedAuthor);
        await _dbContextLite.SaveChangesAsync();
        //act


        Author? author = await _dbContextLite.Authors.FindAsync(expectedAuthor.Id);
        if (author is not null)
            _repository.RemoveAuthor(author);
        bool result = await _dbContextLite.SaveChangesAsync() > 0;

        //assert
        result.Should().BeTrue();
        _dbContextLite.Authors.ToList().Should().HaveCount(0);

    }


    [Fact]
    public async Task Delete_ShouldRetrunFalse()
    {

        //ararnge
        Author expectedAuthor = AuthorScenario.CreateAuthor();

        Author expectedFake = new (
            Guid.NewGuid().ToString(),
            expectedAuthor.Name,
            Guid.NewGuid().ToString(),
            _faker.Person.Email
        );


        await _dbContextLite.Authors.AddAsync(expectedAuthor);
        await _dbContextLite.SaveChangesAsync();

        //act

        Author? author  = await _dbContextLite.Authors.FindAsync(expectedFake.Id);
        if (author is not null)
            _repository.RemoveAuthor(author);

        
        bool result = await _dbContextLite.SaveChangesAsync() > 0;


        //assert
        result.Should().BeFalse();
    }


}
