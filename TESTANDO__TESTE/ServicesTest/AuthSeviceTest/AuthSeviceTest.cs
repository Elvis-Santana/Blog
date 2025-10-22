using Application.Dtos.Models;
using Application.IServices;
using Application.Services.AuthSevice;
using Bogus;
using Domain.Entities;
using Domain.IRepository.IAuthorRepository;
using Domain.ObjectValues;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.Linq.Expressions;

namespace TESTANDO__TESTE.ServicesTest.AuthSeviceTest;

public class AuthSeviceTest
{
    private readonly IConfiguration _configuration;
    private readonly IAuthorRepository _moqAuthorRepository;
    

    private readonly Faker _faker = new("pt_BR");
    public AuthSeviceTest( )
    {
        this._configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"Api:SecretKey","b2f8c3e1a4d6e7f9b0c2d4e6f8a1b3c5d7e9f0a2b4c6d8e0f1a3b5c7d9e1f3a5/@--+457*&n"}

            }).Build();

        _moqAuthorRepository = Substitute.For<IAuthorRepository>();



    }

  
    [Fact]
    public async Task CreateToKen__ShouldReturnTokenEmpty()
    {
        //arrange
        var login = new Login(string.Empty);


        //act
        IAuthSevice authSevice = new AuthSevice(this._configuration, _moqAuthorRepository);

        Token tokenJWT = await authSevice.CriateToken(login);

        //assert
        tokenJWT.token.Should().BeEmpty();
    }


    [Fact]
    public async Task CreateToKen__ShouldReturnToken()
    {
        //arrange

        string password = Guid.NewGuid().ToString();
        var login = new Login(password);

        var author = (Author)new AuthorCreateDTO(new(_faker.Person.FirstName, string.Empty), password);

        
        _moqAuthorRepository.GetByExpression(Arg.Any<Expression<Func<Author, bool>>>()).Returns(Task.FromResult(author));


        //act
        IAuthSevice authSevice = new AuthSevice(this._configuration, _moqAuthorRepository);

        Token tokenJWT = await authSevice.CriateToken(login);

        //assert
        tokenJWT.token.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Verify__ShouldReturnFalse()
    {
      var password = Guid.NewGuid().ToString();
      var passwordError = Guid.NewGuid().ToString();

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
      var author =  Author.Factory.CriarAuthor(
          new(_faker.Person.FirstName, string.Empty),
          passwordHash
      );


        var result = author.Verify(passwordError);

        result.Should().BeFalse();


    }

    [Fact]
    public async Task Verify__ShouldReturnTrue()
    {
        var password = Guid.NewGuid().ToString();

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var author = Author.Factory.CriarAuthor(
            new(_faker.Person.FirstName, string.Empty),
            passwordHash
        );


        var result = author.Verify(password);

        result.Should().BeTrue();


    }


    [Fact]
    public async Task Validation__ShouldReturnFalse()
    {
        //arrange
        string password = Guid.NewGuid().ToString();
        var login = new Login(password);

        var author = (Author)new AuthorCreateDTO(new(_faker.Person.FirstName, string.Empty), password);


        //_moqAuthorRepository.GetByExpression(Arg.Any<Expression<Func<Author, bool>>>())
        //    .Returns(Task.FromResult(author));

        IAuthSevice authSevice = new AuthSevice(this._configuration, _moqAuthorRepository);

        Token tokenJWT = Token.Factory.CreateToken(Guid.NewGuid().ToString());

        //act

        var result =await authSevice.Validation(tokenJWT);

        result.Should().BeFalse();

    }

    [Fact]
    public async Task Validation__ShouldReturnTrue()
    {
        //arrange
        string password = Guid.NewGuid().ToString();
        var login = new Login(password);

        var author = (Author)new AuthorCreateDTO(new(_faker.Person.FirstName, string.Empty), password);


        _moqAuthorRepository.GetByExpression(Arg.Any<Expression<Func<Author, bool>>>())
            .Returns(Task.FromResult(author));

        IAuthSevice authSevice = new AuthSevice(this._configuration, _moqAuthorRepository);

        Token tokenJWT = await authSevice.CriateToken(login);

        //act

        var result = await authSevice.Validation(tokenJWT);

        result.Should().BeTrue();

    }

}
