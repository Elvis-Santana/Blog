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

namespace TESTANDO__TESTE.ServicesTest.AuthorServiceTest;

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
    public async Task Create_ParamsIsValid_ShouldRetrunTrue()
    {
        //arrange
        IServiceAuthor serviceAuthor =  new AuthorService(mackAuthorRepository, new AuthorValidator());

        FullName fullName = new FullName(this._faker.Person.FirstName, this._faker.Person.LastName);
        AddAuthorInputModel author = new(fullName);
        var expecteAuthor = (Author)author;

        mackAuthorRepository.Create(Arg.Any<Author>()).Returns(Task.FromResult(expecteAuthor));



        //act
        var result = await serviceAuthor.CreateAuthor(author);

        //assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().BeAssignableTo<AuthorViewModel>();
        result.AsT0.Should().BeEquivalentTo(expecteAuthor);
       

    }


    [Fact]
    public async Task Create_ParamsIsInvalidis_ShouldRetrunErrors()
    {
        //arrange
        AuthorService createAuthor = new AuthorService(mackAuthorRepository, new AuthorValidator());
        AddAuthorInputModel author = null;
        //act
        var result = await createAuthor.CreateAuthor(author);

        //assert

        result.Switch(
            e => e.Should().BeNull(),
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
    public async Task GetAuthor_ShouldReturnListAuthor()
    {
        //arrange
        FullName  expectedFullName = new FullName(this._faker.Person.FirstName, this._faker.Person.LastName);
        var expectedAuthor =  Author.Factory.CriarAuthor(expectedFullName);

        Category expectedCategory = Category.Factory.CreateCategory(expectedAuthor.Id, this._faker.Lorem.Paragraph(1));

        List<Author> expectedListAuthor = new List<Author>() { 
            new Author(expectedAuthor.Id, expectedFullName,new List<Post>()
             {
                    Post.Factory.CreatePost(
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
        result.Should().BeEquivalentTo(expectedListAuthor.Map());
        result[0].Post[0].AuthorId.Should().Be(expectedListAuthor[0].Id);
        result[0].Post[0].Category.Should().BeEquivalentTo(expectedCategory.Map());
        result[0].Post[0].CategoryId.Should().Be(expectedCategory.Id);

    }


    [Fact]
    public async Task Update_ShouldRetrunAuthorNotFound()
    {

        //arrange
        FullName expectedName = new(_faker.Person.FirstName,_faker.Person.LastName);
        string idError = Guid.NewGuid().ToString();

        AddAuthorInputModel addAuthorInputModel = new(expectedName);
        Author author =  Author.Factory.CriarAuthor( new ("a",""));
        

      


        //act
        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorValidator());

        var result = await serviceAuthor.Update(addAuthorInputModel, idError);


        //assert
           result.AsT1.errors.Should().HaveCount(1);



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
    public async Task GetById_ShouldRetrunNotFound()
    {
        //arrange
        FullName expectedName = new(_faker.Person.FirstName, _faker.Person.LastName);
        string expectedGuid = Guid.NewGuid().ToString();
        string idError = Guid.NewGuid().ToString();

        Author author = new Author(expectedGuid, expectedName);


        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorValidator());

        //act


        var result = await serviceAuthor.GetById(idError);


        //assert

        result.AsT1.errors.Should().HaveCount(1);
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

    [Fact]
    public async Task DeleteById__ShouldRetrunNotFound()
    {
        //arrange
        FullName expectedName = new(_faker.Person.FirstName, _faker.Person.LastName);
        string expectedGuid = Guid.NewGuid().ToString();
        Author author = new Author(expectedGuid, expectedName);
        string isError = Guid.NewGuid().ToString();


        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorValidator());
        //act


        var result = await serviceAuthor.DeleteById(isError);


        //assert

        result.AsT1.errors.Should().HaveCount(1);

    }


   

}
