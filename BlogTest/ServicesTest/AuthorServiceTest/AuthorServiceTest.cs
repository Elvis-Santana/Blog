using Application.Dtos.Models;
using Application.IServices;
using Application.Services.AuthorService;
using Application.Validators.Validator.AuthorValidator;
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
        IServiceAuthor serviceAuthor =  new AuthorService(mackAuthorRepository, new AuthorCreateValidator());

        var fullName = new FullName(this._faker.Person.FirstName, this._faker.Person.LastName);
        var author = new AuthorCreateDTO(fullName, Guid.NewGuid().ToString(), _faker.Person.Email);
        var expecteAuthor = (Author)author;

        
        mackAuthorRepository.Create(Arg.Any<Author>()).Returns(Task.FromResult(expecteAuthor));


          //act
          var result = await serviceAuthor.CreateAuthor(author);

        //assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().BeAssignableTo<AuthorReadDTO>();
        result.AsT0.Id.Should().NotBeNull();
        result.AsT0.Name.Should().Be(author.Name);



    }


    [Fact]
    public async Task Create_ParamsIsInvalidis_ShouldRetrunErrors()
    {
        //arrange
        AuthorService createAuthor = new AuthorService(mackAuthorRepository, new AuthorCreateValidator());
        AuthorCreateDTO author = null;
        //act
        var result = await createAuthor.CreateAuthor(author);

        //assert

        result.Switch(
            e => e.Should().BeNull(),
            err => err.errors.ToList().ForEach(x =>
            {
                this._testOutputHelper.WriteLine(x.messagem);
                this._testOutputHelper.WriteLine(x.field);

                x.messagem.Should().Be(AuthorMsg.AuthorErroNull);
                x.field.Should().Be(nameof(AuthorCreateDTO));

            })
        );
    }


    [Fact]
    public async Task GetAuthor_ShouldReturnListAuthor()
    {
        //arrange
        FullName  expectedFullName = new FullName(this._faker.Person.FirstName, this._faker.Person.LastName);
        var expectedAuthor =  Author.Factory.CriarAuthor(expectedFullName, Guid.NewGuid().ToString(), _faker.Person.Email);

        Category expectedCategory = Category.Factory.CreateCategory(expectedAuthor.Id, this._faker.Lorem.Paragraph(1), _faker.Person.Email);


    
        IEnumerable<Author> expectedListAuthor = new List<Author>() { 
            new Author(expectedAuthor.Id, expectedFullName,[
                 Post.Factory.CreatePost(
                        this._faker.Lorem.Paragraph(1),
                        this._faker.Lorem.Paragraph(1),
                        DateTime.Now,
                        expectedCategory,
                        expectedAuthor
                    )
                ],
            Guid.NewGuid().ToString(),
            _faker.Person.Email
            )


        };



        mackAuthorRepository.GetAllAsync().Returns(Task.FromResult(expectedListAuthor));


        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorCreateValidator());


        //act
        var result = (await serviceAuthor.GetAuthor()).ToList() ;

        //assert

        result.Should().BeAssignableTo<List<AuthorReadDTO>>();
        result.Should().BeEquivalentTo(expectedListAuthor.Map());
        result[0].Post[0].AuthorId.Should().Be((expectedListAuthor.ToList())[0].Id);
        result[0].Post[0].Category.Should().BeEquivalentTo(expectedCategory.Map());
        result[0].Post[0].CategoryId.Should().Be(expectedCategory.Id);

    }


    [Fact]
    public async Task Update_ShouldRetrunAuthorNotFound()
    {

        //arrange
        FullName expectedName = new(_faker.Person.FirstName,_faker.Person.LastName);
        string idError = Guid.NewGuid().ToString();

        AuthorCreateDTO addAuthorInputModel = new(expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);
        Author author =  Author.Factory.CriarAuthor( new ("a",""), Guid.NewGuid().ToString(), _faker.Person.Email);
        

      


        //act
        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorCreateValidator());

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
        Author author = new Author(expectedGuid, expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);

        mackAuthorRepository.  GetById(expectedGuid).Returns(Task.FromResult(author));

        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorCreateValidator());
        //act


        var result = await serviceAuthor.GetById(expectedGuid);


        //assert

        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(expectedGuid);
        result.AsT0.Name.Should().Be(expectedName);
        result.AsT0.Post.Should().HaveCount(0);

        //result.AsT0.Should().BeEquivalentTo(author);


    }


    [Fact]
    public async Task GetById_ShouldRetrunNotFound()
    {
        //arrange
        FullName expectedName = new(_faker.Person.FirstName, _faker.Person.LastName);
        string expectedGuid = Guid.NewGuid().ToString();
        string idError = Guid.NewGuid().ToString();

        Author author = new Author(expectedGuid, expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);


        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorCreateValidator());

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
        Author author = new Author(expectedGuid, expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);

        mackAuthorRepository.DeleteById(expectedGuid).Returns(Task.FromResult(true));

        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorCreateValidator());
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
        Author author = new Author(expectedGuid, expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);
        string isError = Guid.NewGuid().ToString();


        IServiceAuthor serviceAuthor = new AuthorService(mackAuthorRepository, new AuthorCreateValidator());
        //act


        var result = await serviceAuthor.DeleteById(isError);


        //assert

        result.AsT1.errors.Should().HaveCount(1);

    }
    [Fact]
    public async Task DeleteById__ShouldRetrunNotFounda()
    {




        var resi  = new Author("expectedGuid", new FullName("",""), Guid.NewGuid().ToString(), _faker.Person.Email);;
        var a = resi.GetType();

        this._testOutputHelper.WriteLine(a.Name);
    }


  



}
