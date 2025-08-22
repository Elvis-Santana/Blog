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
        var expectedAuthor = new Author(expectedFullName);

        Category expectedCategory = new Category(expectedAuthor.Id, this._faker.Lorem.Paragraph(1));

        List<Author> expectedListAuthor = new List<Author>() { 
            new Author(expectedAuthor.Id, expectedFullName,new List<Post>()
             {
                    new Post(
                        this._faker.Lorem.Paragraph(1),
                        this._faker.Lorem.Paragraph(1),
                        DateTime.Now,
                        expectedCategory, 
                        expectedAuthor
                    )
            }) 
       
        };



        mackAuthorRepository.GetAllAsync().Returns(Task.FromResult(expectedListAuthor));


        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorValidator());


        //act
        var result = await serviceAuthor.GetAuthor();

        //assert

        result.Should().BeAssignableTo<List<AuthorViewModel>>();
        result.Should().BeEquivalentTo(expectedListAuthor.Adapt<List<AuthorViewModel>>());
        result[0].Post[0].AuthorId.Should().Be(expectedListAuthor[0].Id);
        result[0].Post[0].Category.Should().BeEquivalentTo(expectedCategory.Adapt<CategoryViewModel>());
        result[0].Post[0].CategoryId.Should().Be(expectedCategory.Id);

    }


    [Fact]
    public async Task Update_ShouldRetrunAuthorUpdated()
    {

        //arrange
        FullName expectedName = new(_faker.Person.FirstName,_faker.Person.LastName);
        string expectedGuid = Guid.NewGuid().ToString();

        AddAuthorInputModel addAuthorInputModel = new(expectedName);
        Author author = new Author(expectedGuid, new FullName("a",""));

        mackAuthorRepository.Update(Arg.Any<Author>(), author.Id).Returns(Task.FromResult(new Author(author.Id,addAuthorInputModel.Name, author.Post)));

        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorValidator());
        //act


        var result = await serviceAuthor.Update(addAuthorInputModel, author.Id);


        //assert

        result.IsT0.Should().BeTrue();
        result.AsT0.Should().BeAssignableTo<AuthorViewModel>();



    }

    [Fact]
    public async Task GetById_ShouldRetrunAuthor()
    {

        //arrange
        FullName expectedName = new(_faker.Person.FirstName, _faker.Person.LastName);
        string expectedGuid = Guid.NewGuid().ToString();
        Author author = new Author(expectedGuid, expectedName);

        mackAuthorRepository.GetById(expectedGuid).Returns(Task.FromResult(author));

        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorValidator());
        //act


        var result = await serviceAuthor.GetById(expectedGuid);


        //assert

        result.IsT0.Should().BeTrue();
        result.AsT0.Should().BeEquivalentTo(author);


    }


    [Fact]
    public async Task DeleteById__ShouldRetrunTrue()
    {
        //arrange
        FullName expectedName = new(_faker.Person.FirstName, _faker.Person.LastName);
        string expectedGuid = Guid.NewGuid().ToString();
        Author author = new Author(expectedGuid, expectedName);

        mackAuthorRepository.DeleteById(expectedGuid).Returns(Task.FromResult(true));

        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorValidator());
        //act


        var result = await serviceAuthor.DeleteById(expectedGuid);


        //assert

        result.IsT0.Should().BeTrue();

    }
}
