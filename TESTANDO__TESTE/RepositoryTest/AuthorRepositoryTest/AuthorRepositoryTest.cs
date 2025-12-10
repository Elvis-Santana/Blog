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
        var expectedNameList = new List<FullName>()
        {
            new(this._faker.Person.FirstName, this._faker.Person.LastName),
            new(this._faker.Person.FirstName, this._faker.Person.LastName)
        };
       

        var expecteAuthorId1 = Guid.NewGuid().ToString();
        var expecteAuthorId2 = Guid.NewGuid().ToString();

        var category = Category.Factory.CreateCategory(expecteAuthorId1, "name", _faker.Person.Email);

        var expectedAuthoes = new List<Author>()
        {
            new Author(expecteAuthorId1,expectedNameList[0], Guid.NewGuid().ToString(), _faker.Person.Email),
            new Author(expecteAuthorId2,expectedNameList[1], Guid.NewGuid().ToString(), _faker.Person.Email)
        };

        using var dbContext = new DbContextLite(this._dbContextOptions);

        await dbContext.Authors.AddRangeAsync(expectedAuthoes);
        await dbContext.SaveChangesAsync();

        await dbContext.Category.AddAsync(category);
        await dbContext.SaveChangesAsync();

        var expectedPost1 =
             Post.Factory.CreatePost (this._faker.Person.FirstName, this._faker.Lorem.ToString()!, new DateTime(), category.Id, expecteAuthorId1);
        var expectedPost2 =
            Post.Factory.CreatePost(this._faker.Person.FirstName, this._faker.Lorem.ToString()!, new DateTime(), category.Id, expecteAuthorId2);

        await dbContext.Posts.AddRangeAsync(new List<Post>() { expectedPost1,expectedPost2});
        await dbContext.SaveChangesAsync();

        //act
        AuthorRepository authorRepository = new(dbContext);
        var result = (await authorRepository.GetAllAsync()).ToList(); ;

        //assert
        result.Count.Should().Be(2);
    
        for (int i = 0; i < result.Count; i++)
            result[i].Name.Should().Be(expectedNameList[i]);
    }



    [Fact]
    public async Task Create_ShouldSaveAuthorToDatabase()
    {
        //arrange
        var expectedId = Guid.NewGuid().ToString();
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);
        var expecteAuthorId1 = Guid.NewGuid().ToString();

        var category = Category.Factory.CreateCategory(expecteAuthorId1, "name" , _faker.Person.Email);
        List<Post> expectedIdPosts = new List<Post>
        {
                 Post.Factory.CreatePost(
                     this._faker.Person.FirstName,
                     this._faker.Lorem.ToString() !,
                     new DateTime(),
                     category.Id ,
                     expecteAuthorId1
                     ), 
        };

        //act
        Author author = new(expectedId, expectedName, expectedIdPosts, Guid.NewGuid().ToString(), _faker.Person.Email);
        
        using var dbContext = new DbContextLite(this._dbContextOptions);



        AuthorRepository authorRepository = new(dbContext);
        var result = await authorRepository.Create(author);

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

        var category = Category.Factory.CreateCategory(expecteAuthorId1, "name");

        var expectedAuthoes = new List<Author>()
        {
            new Author(expecteAuthorId1,expectedNameList[0], Guid.NewGuid().ToString(), _faker.Person.Email),
            new Author(expecteAuthorId2,expectedNameList[1], Guid.NewGuid().ToString(), _faker.Person.Email )
        };

        using var dbContext = new DbContextLite(this._dbContextOptions);

        await dbContext.Authors.AddRangeAsync(expectedAuthoes);
        await dbContext.SaveChangesAsync();

        await dbContext.Category.AddAsync(category);
        await dbContext.SaveChangesAsync();

        var expectedPost1 =
            Post.Factory.CreatePost(this._faker.Person.FirstName, this._faker.Lorem.ToString()!, new DateTime(), category.Id, expecteAuthorId1);
        var expectedPost2 =
            Post.Factory.CreatePost(this._faker.Person.FirstName, this._faker.Lorem.ToString()!, new DateTime(), category.Id, expecteAuthorId2);

        var expectedPost = new List<Post>() { expectedPost1, expectedPost2 };

        await dbContext.Posts.AddRangeAsync(expectedPost);
        await dbContext.SaveChangesAsync();


        //act

        var authorRepository = new AuthorRepository(dbContext);
         
        Author result = await authorRepository.GetById(expectedAuthoes.ToArray()[0].Id);


        //assert

        result.Id.Should().Be(expectedAuthoes[0].Id);
        result.Post[0].Should().BeEquivalentTo(expectedPost.ToArray()[0]);
        result.Name.Should().Be(expectedAuthoes[0].Name);
    }



    [Fact]
    public async Task GetById_ShouldReturnNull()
    {
        //arrange



        using var dbContext = new DbContextLite(this._dbContextOptions);

        dbContext.Authors.AddRange([
            Author.Factory.CriarAuthor(
                 new (
                     this._faker.Person.FirstName,
                     this._faker.Person.LastName
                 ),
                                      Guid.NewGuid().ToString(),
                                      _faker.Person.Email

            ),

            Author.Factory.CriarAuthor(
                  new (
                      this._faker.Person.FirstName,
                      this._faker.Person.LastName
                  ),
                Guid.NewGuid().ToString(),
                _faker.Person.Email

            )

        ]);

        var idError = Guid.NewGuid().ToString();

        //act
        var authorRepository = new AuthorRepository(dbContext);

        Author result = await authorRepository.GetById(idError);

        //assert  

        result.Should().BeNull();
    }


    [Fact]
    public async Task GetByExpression_ShouldExpressionNull()
    {
        //arrange



        using var dbContext = new DbContextLite(this._dbContextOptions);

        dbContext.Authors.AddRange([
            Author.Factory.CriarAuthor(
                 new (
                     this._faker.Person.FirstName,
                     this._faker.Person.LastName
                 ),
                                      Guid.NewGuid().ToString(), _faker.Person.Email

            ),

            Author.Factory.CriarAuthor(
                  new (
                      this._faker.Person.FirstName,
                      this._faker.Person.LastName
                  ),
                Guid.NewGuid().ToString(),
                _faker.Person.Email

            )

        ]);

        var idError = Guid.NewGuid().ToString();

        //act
        var authorRepository = new AuthorRepository(dbContext);

        Author? result = await authorRepository.GetByExpression((a => a.Id.Equals(idError) ));

        //assert  

        result.Should().BeNull();
    }
    [Fact]
    public async Task GetByExpression_ShouldExpressionAuthor()
    {
        //arrange



        using var dbContext = new DbContextLite(this._dbContextOptions);
        var idExpected = Guid.NewGuid().ToString();

        dbContext.Authors.AddRange([
            Author.Factory.CriarAuthor(
                 new (
                     this._faker.Person.FirstName,
                     this._faker.Person.LastName
                 ),
                                      idExpected,
                                      _faker.Person.Email

            ),

            Author.Factory.CriarAuthor(
                  new (
                      this._faker.Person.FirstName,
                      this._faker.Person.LastName
                  ),
                Guid.NewGuid().ToString(),
                _faker.Person.Email

            )

        ]);


        //act
        var authorRepository = new AuthorRepository(dbContext);

        Author? result = await authorRepository.GetByExpression((a => a.Id.Equals(idExpected)));

        //assert  

        result.Should().BeNull();
    }
    [Fact]
    public async Task Update_ShouldAuthorAtualizado()
    {
        //ararnge

        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);
        FullName expectedUpdatedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);

        var expectedAuthor = new Author(Guid.NewGuid().ToString(), expectedName, Guid.NewGuid().ToString(),_faker.Person.Email);
        var expectedUpdateAuthor = new Author(expectedAuthor.Id, expectedUpdatedName, Guid.NewGuid().ToString(), _faker.Person.Email);



        using var dbContext = new DbContextLite(this._dbContextOptions);

        await dbContext.Authors.AddAsync(expectedAuthor);
        await dbContext.SaveChangesAsync();



        //act

        var authorRepository = new AuthorRepository(dbContext);

        Author result = await authorRepository.Update(expectedUpdateAuthor, expectedUpdateAuthor.Id);


        //assert

        result.Id.Should().NotBeEmpty();
        result.Id.Should().Be(expectedUpdateAuthor.Id);
        result.Name.Should().BeEquivalentTo(expectedUpdateAuthor.Name);

    }



    [Fact]
    public async Task Update_ShouldAuthorNaoAtualizado()
    {

        //ararnge
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);
        FullName expectedUpdatedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);

        var expectedAuthor = new Author(Guid.NewGuid().ToString(), expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);
        var expectedUpdateAuthor = new Author(expectedAuthor.Id, expectedUpdatedName, Guid.NewGuid().ToString(), _faker.Person.Email);

        var idError = Guid.NewGuid().ToString();

        using var dbContext = new DbContextLite(this._dbContextOptions);

        await dbContext.Authors.AddAsync(expectedAuthor);
        await dbContext.SaveChangesAsync();

        //act
        var authorRepository = new AuthorRepository(dbContext);

        Author result = await authorRepository.Update(expectedUpdateAuthor, idError);

        //assert

        result.Should().BeNull();


    }


    [Fact]
    public async Task Delete_ShouldRetrunTrue()
    {
        //ararnge
        
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);
     

        var expectedAuthor = new Author(Guid.NewGuid().ToString(), expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);

        using var dbContext = new DbContextLite(this._dbContextOptions);

        await dbContext.Authors.AddAsync(expectedAuthor);
        await dbContext.SaveChangesAsync();



        //act

        var authorRepository = new AuthorRepository(dbContext);

        bool result = await authorRepository.DeleteById(expectedAuthor.Id);


        //assert

        result.Should().BeTrue();
        dbContext.Authors.ToList().Should().HaveCount(0);

    }


    [Fact]
    public async Task Delete_ShouldRetrunFalse()
    {

        //ararnge
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);


        var expectedAuthor = new Author(Guid.NewGuid().ToString(), expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);
        var idError = Guid.NewGuid().ToString();
        using var dbContext = new DbContextLite(this._dbContextOptions);

        await dbContext.Authors.AddAsync(expectedAuthor);
        await dbContext.SaveChangesAsync();

        //act
        var authorRepository = new AuthorRepository(dbContext);

        bool result = await authorRepository.DeleteById(idError);



        //assert
        result.Should().BeFalse();
        dbContext.Authors.ToList().Should().HaveCount(1);



    }


}
