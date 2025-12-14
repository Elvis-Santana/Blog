using Bogus;
using Domain.Entities;
using Domain.IRepository.IAuthorRepository;
using Domain.ObjectValues;
using FluentAssertions;
using Infrastructure.Db;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace TESTANDO__TESTE.RepositoryTest.AuthorRepositoryTest;


public class AuthorRepositoryTest
{
    private readonly DbContextOptions<DbContextLite> _dbContextOptions;
    private readonly Faker _faker = new ("pt_BR");
    private readonly  DbContextLite _dbContextLite;
    private readonly IAuthorRepository _repository;

    public AuthorRepositoryTest()
    {

        this._dbContextOptions  = new DbContextOptionsBuilder<DbContextLite>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

        _dbContextLite = new DbContextLite(_dbContextOptions);
        _repository = new AuthorRepository(_dbContextLite);

    }

    [Fact]
    public async  Task GetAllAsync_ShouldReturnListAuthors()
    {
        //arrange
        List<FullName> expectedNameList = [
            new(this._faker.Person.FirstName, this._faker.Person.LastName),
            new(this._faker.Person.FirstName, this._faker.Person.LastName)
        ];
       
        var expecteAuthorId1 = Guid.NewGuid().ToString();
        var expecteAuthorId2 = Guid.NewGuid().ToString();

        var category = Category.Factory.CreateCategory(expecteAuthorId1, _faker.Person.FullName, _faker.Person.Email);

        List<Author> expectedAuthoes = [
            new (expecteAuthorId1,expectedNameList[0], Guid.NewGuid().ToString(), _faker.Person.Email),
            new (expecteAuthorId2,expectedNameList[1], Guid.NewGuid().ToString(), _faker.Person.Email)
        ];
      

        await _dbContextLite.Authors.AddRangeAsync(expectedAuthoes);
        await _dbContextLite.SaveChangesAsync();

        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.SaveChangesAsync();

        var expectedPost1 = Post.Factory
            .CreatePost (
            this._faker.Person.FirstName,
            this._faker.Lorem.ToString()!,
            new DateTime(),
            category.Id,
            expecteAuthorId1
            );

        var expectedPost2 = Post.Factory
            .CreatePost(
            this._faker.Person.FirstName,
            this._faker.Lorem.ToString()!,
            new DateTime(), 
            category.Id,
            expecteAuthorId2
            );

        await _dbContextLite.Posts.AddRangeAsync(expectedPost1, expectedPost2);
        await _dbContextLite.SaveChangesAsync();

        //act
        var result = (await _repository.GetAllAuthorAsync()).ToList(); ;

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

        var category = Category.Factory.CreateCategory(expecteAuthorId1, this._faker.Person.FirstName, _faker.Person.Email);
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


        await _dbContextLite    .Authors.AddRangeAsync(expectedAuthoes);
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

        _dbContextLite.Authors.AddRange([
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

        Author result = await _repository.GetAuthorByIdAsync(idError);

        //assert  

        result.Should().BeNull();
    }


    [Fact]
    public async Task GetByExpression_ShouldExpressionNull()
    {
        //arrange
        _dbContextLite.Authors.AddRange([
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

        Author? result = await _repository.GetByExpression((a => a.Id.Equals(idError) ));

        //assert  

        result.Should().BeNull();
    }
    [Fact]
    public async Task GetByExpression_ShouldExpressionAuthor()
    {
        //arrange



        var idExpected = Guid.NewGuid().ToString();

        _dbContextLite.Authors.AddRange([
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

        Author? result = await _repository.GetByExpression((a => a.Id.Equals(idExpected)));

        //assert  
        result.Should().BeNull();
    }

    [Fact]
    public async Task Update_ShouldAuthorAtualizado()
    {
        //ararnge
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);
        FullName expectedUpdatedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);

        var expectedAuthor = new Author(Guid.NewGuid().ToString(), expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);
        var expectedUpdateAuthor = new Author(expectedAuthor.Id, expectedUpdatedName, Guid.NewGuid().ToString(), _faker.Person.Email);


        await _dbContextLite.Authors.AddAsync(expectedAuthor);
        await _dbContextLite.SaveChangesAsync();
        //act

        var IUnitOfWork = new UnitOfWork(_dbContextLite);
        expectedAuthor.UpdateName(expectedAuthor.Name);

        await IUnitOfWork.SaveAsync();

        var result = await _dbContextLite.Authors.FindAsync(expectedAuthor.Id);

        //assert
        result!.Id.Should().NotBeEmpty();
        result!.Id.Should().Be(expectedUpdateAuthor.Id);
        result!.Name.Should().BeEquivalentTo(expectedUpdateAuthor.Name);
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
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);
        var expectedAuthor = new Author(Guid.NewGuid().ToString(), expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);

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
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);
        var expectedAuthor = new Author(Guid.NewGuid().ToString(), expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);
        var expectedFake = new Author(Guid.NewGuid().ToString(), expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);


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
