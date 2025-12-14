using Application.Dtos.Models;
using Application.IServices;
using Application.Services.AuthorService;
using Application.Validators.Validator.AuthorValidator;
using Bogus;
using Domain.Entities;
using Domain.IRepository.IAuthorRepository;
using Domain.ObjectValues;
using FluentAssertions;
using Infrastructure.UnitOfWork;
using Mapster;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.ServicesTest.AuthorServiceTest;

public class AuthorServiceTest
{
    private readonly IAuthorRepository _mackAuthorRepository;
    private readonly IUnitOfWork _mackIUnitOfWork;
    private readonly IServiceAuthor _serviceAuthor;

    private readonly Faker _faker = new("pt_BR");
    private readonly ITestOutputHelper _testOutputHelper;


    public AuthorServiceTest(ITestOutputHelper testOutputHelper) 
    { 
    
        _testOutputHelper = testOutputHelper;
        _mackAuthorRepository = Substitute.For<IAuthorRepository>();
        _mackIUnitOfWork = Substitute.For<IUnitOfWork>();
        _serviceAuthor = new AuthorService(_mackAuthorRepository, new AuthorCreateValidator(), _mackIUnitOfWork);

    }

    [Fact]
    public async Task Create_ParamsIsValid_ShouldRetrunTrue()
    {
        //arrange
       

        var fullName = new FullName(this._faker.Person.FirstName, this._faker.Person.LastName);
        var authorDTO = new AuthorCreateDTO(fullName, Guid.NewGuid().ToString(), _faker.Person.Email);
        var expecteAuthor = (Author)authorDTO;

        
        _mackAuthorRepository.CreateAuthorAsync(Arg.Any<Author>()).Returns(Task.FromResult(expecteAuthor));
        _mackIUnitOfWork.SaveAsync().Returns(Task.FromResult(true));

          //act
          var result= await _serviceAuthor.CreateAuthorAsync(authorDTO);

        //assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Should().BeAssignableTo<AuthorReadDTO>();
        result.AsT0.Id.Should().NotBeNull();
        result.AsT0.Name.Should().Be(authorDTO.Name);
        await _mackAuthorRepository.Received(1).CreateAuthorAsync(Arg.Any<Author>());
        await _mackIUnitOfWork.Received(1).SaveAsync();




    }


    [Fact]
    public async Task Create_ParamsIsInvalidis_ShouldRetrunErrors()
    {
        //arrange
      
        AuthorCreateDTO author = null;
        //act
        var result = await _serviceAuthor.CreateAuthorAsync(author);

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
        await _mackIUnitOfWork.Received(0).SaveAsync();

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



        _mackAuthorRepository.GetAllAuthorAsync().Returns(Task.FromResult(expectedListAuthor));




        //act
        var result = (await _serviceAuthor.GetAllAuthorAsync()).ToList() ;

        //assert

        result.Should().BeAssignableTo<List<AuthorReadDTO>>();
        result.Should().BeEquivalentTo(expectedListAuthor.Map());
        result[0].Post[0].AuthorId.Should().Be((expectedListAuthor.ToList())[0].Id);
        result[0].Post[0].Category.Should().BeEquivalentTo(expectedCategory.Map());
        result[0].Post[0].CategoryId.Should().Be(expectedCategory.Id);

    }


    [Fact]
    public async Task Update_ShouldRetrunAuthorUpdated()
    {

        //arrange
        FullName expectedName = new(_faker.Person.FirstName, _faker.Person.LastName);

        AuthorCreateDTO addAuthorInputModel = new(expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);
        Author author = new Author(Guid.NewGuid().ToString(), expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);

        //act
     
        _mackAuthorRepository.GetAuthorByIdAsync(Arg.Any<string>()).Returns(Task.FromResult(author));
        _mackIUnitOfWork.SaveAsync().Returns(Task.FromResult(true));

        var result = await _serviceAuthor.UpdateAuthorAsync(addAuthorInputModel, author.Id);

        //assert
        result.AsT0.Id.Should();
        result.AsT0.Name.Should().Be(addAuthorInputModel.Name);

        await _mackAuthorRepository.Received(1).GetAuthorByIdAsync(Arg.Any<string>());
        await _mackIUnitOfWork.Received(1).SaveAsync();
    }

    [Fact]
    public async Task Update_ShouldRetrunAuthorNotFound()
    {

        //arrange
        FullName expectedName = new(_faker.Person.FirstName,_faker.Person.LastName);
        string idError = Guid.NewGuid().ToString();

        AuthorCreateDTO authorCreateDTO = new(expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);
        
        //act
      

        var result = await _serviceAuthor.UpdateAuthorAsync(authorCreateDTO, idError);

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

        _mackAuthorRepository.GetAuthorByIdAsync(Arg.Any<string>()).Returns(Task.FromResult(author));

        //act


        var result = await _serviceAuthor.GetAuthorByIdAsync(expectedGuid);


        //assert

        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(expectedGuid);
        result.AsT0.Name.Should().Be(expectedName);
        result.AsT0.Post.Should().HaveCount(0);

        await  _mackAuthorRepository.Received(1).GetAuthorByIdAsync(Arg.Any<string>());

    }


    [Fact]
    public async Task GetById_ShouldRetrunNotFound()
    {
        //arrange
        FullName expectedName = new(_faker.Person.FirstName, _faker.Person.LastName);
        string expectedGuid = Guid.NewGuid().ToString();
        string idError = Guid.NewGuid().ToString();

        Author author = new Author(expectedGuid, expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);



        //act


        var result = await _serviceAuthor.GetAuthorByIdAsync(idError);


        //assert

        result.AsT1.errors.Should().HaveCount(1);
    }



    [Fact]
    public async Task RemoveAuthorById__ShouldRetrunTrue()
    {
        //arrange
        FullName expectedName = new(_faker.Person.FirstName, _faker.Person.LastName);
        string expectedGuid = Guid.NewGuid().ToString();
        Author author = new Author(expectedGuid, expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);

        _mackAuthorRepository.GetAuthorByIdAsync(Arg.Any<string>()).Returns(Task.FromResult(author));

        _mackIUnitOfWork.SaveAsync().Returns(Task.FromResult(true));
        //act


        var result = await _serviceAuthor.RemoveAuthorByIdAsync(expectedGuid);


        //assert

        result.IsT0.Should().BeTrue();
        await _mackAuthorRepository.Received(1).GetAuthorByIdAsync(Arg.Any<string>());
        await _mackIUnitOfWork.Received(1).SaveAsync();


    }

    [Fact]
    public async Task RemoveAuthorById__ShouldRetrunFlase()
    {
        //arrange
        FullName expectedName = new(_faker.Person.FirstName, _faker.Person.LastName);
        string expectedGuid = Guid.NewGuid().ToString();
        Author author = new Author(expectedGuid, expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);
        string isError = Guid.NewGuid().ToString();


        //act


        var result = await _serviceAuthor.RemoveAuthorByIdAsync(isError);


        //assert

        result.AsT1.errors.Should().HaveCount(1);

    }
   




}
