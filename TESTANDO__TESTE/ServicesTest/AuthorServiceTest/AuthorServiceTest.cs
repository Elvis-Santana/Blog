using Application.Dtos.AuthorViewModel;
using Application.Dtos.Models;
using Application.IServices;
using Application.Services.AuthorService;
using Application.Validators.AuthorValidator;
using Bogus;
using Domain.Entities;
using Domain.IRepository.IAuthorRepository;
using Domain.ObjectValues;
using FluentAssertions;
using Mapster;
using NSubstitute;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.UseCaseTest.AuthorUseCaseTest;

public class AuthorServiceTest
{
    private readonly IAuthorRepository mackAuthorRepository;
    private readonly Faker _faker = new("pt_BR");
    private readonly ITestOutputHelper _testOutputHelper;


    public AuthorServiceTest(ITestOutputHelper testOutputHelper) 
    { 
    
        _testOutputHelper = testOutputHelper;
        mackAuthorRepository = Substitute.For<IAuthorRepository>();
    }

    [Fact]
    public async Task CreateAuthorUseCase_ParamsIsValid_ShouldRetrunTrue()
    {
        //arrange
        IServiceAuthor serviceAuthor =  new AuthorService(mackAuthorRepository, new AuthorValidator());

        FullName fullName = new FullName(this._faker.Person.FirstName, this._faker.Person.LastName);
        AddAuthorInputModel author = new(fullName);

        //act
        var result = await serviceAuthor.CreateAuthor(author);

        //assert
        result.IsT0.Should().BeTrue();

    }


    [Fact]
    public async Task CreateAuthorUseCase_ParamsIsInvalidis_ShouldRetrunErrors()
    {
        //arrange
        AuthorService createAuthor = new AuthorService(mackAuthorRepository, new AuthorValidator());
        AddAuthorInputModel author = null;
        //act
        var result = await createAuthor.CreateAuthor(author);

        //assert
        result.IsT1.Should().BeTrue();

        result.Switch(
            e => e.Should().BeFalse(),
            err => err.errors.ForEach(x =>
            {
                this._testOutputHelper.WriteLine(x.messagem);
                this._testOutputHelper.WriteLine(x.field);

                x.messagem.Should().Be(AuthorMsg.AuthorErroNull);
                x.field.Should().Be(nameof(AddAuthorInputModel));

            })
        );
    }


    [Fact]
    public async Task GetAuthorUserCase_ShouldReturnListAuthor()
    {
        //arrange
        FullName  expectedFullName = new FullName(this._faker.Person.FirstName, this._faker.Person.LastName);

        List<Author> expectedPosts = new List<Author>() { new Author( expectedFullName) };

        mackAuthorRepository.GetAllAsync().Returns(Task.FromResult(expectedPosts));
       

        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorValidator());


        //act
        var result = await serviceAuthor.GetAuthor();

        //assert

        result.Should().BeAssignableTo<List<AuthorViewModel>>();
        result.Should().BeEquivalentTo(expectedPosts.Adapt<List<AuthorViewModel>>());




    }
}
