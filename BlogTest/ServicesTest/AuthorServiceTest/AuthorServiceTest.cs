using Application.Dtos.Models;
using Application.IRepository.IAuthorRepository;
using Application.IServices;
using Application.IUnitOfWork;
using Application.Services.AuthorService;
using Application.Validators.Validator.AuthorValidator;
using BlogTest.Scenario;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Mapster;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.ServicesTest.AuthorServiceTest;

public class AuthorServiceTest
{
    private readonly IAuthorRepository _mackAuthorRepository = Substitute.For<IAuthorRepository>();
    private readonly IUnitOfWork _mackIUnitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IServiceAuthor _serviceAuthor;

    private readonly Faker _faker = new("pt_BR");
    private readonly ITestOutputHelper _testOutputHelper;


    public AuthorServiceTest(ITestOutputHelper testOutputHelper) 
    { 
    
        _testOutputHelper = testOutputHelper;
        _serviceAuthor = new AuthorService(_mackAuthorRepository, new AuthorCreateValidator(), _mackIUnitOfWork);

    }

    [Fact]
    public async Task CreateAuthor_ShouldReturnAuthorReadDTO_WhenParamsAreValid()
    {
        //arrange

        var (dto, author) =  AuthorScenario.CreateAuthor_ValidScenarioFormDto();

        _mackAuthorRepository.CreateAuthorAsync(Arg.Any<Author>())
            .Returns(Task.FromResult(author));

        _mackIUnitOfWork.SaveAsync()
            .Returns(Task.FromResult(true));

          //act
        var result= await _serviceAuthor.CreateAuthorAsync(dto);

        //assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().NotBeNull();
        result.AsT0.Name.Should().Be(dto.Name);

        await _mackAuthorRepository.Received(1)
            .CreateAuthorAsync(Arg.Any<Author>());

        await _mackIUnitOfWork.Received(1)
            .SaveAsync();

    }


    [Fact]
    public async Task CreateAuthor_ShouldReturnError_WhenParamsAreInvalid()
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
        await _mackIUnitOfWork.DidNotReceive().SaveAsync();

    }


    [Fact]
    public async Task GetAuthor_ShouldReturnListAuthor()
    {
        //arrange

        IEnumerable<Author> expectedListAuthor = [
            AuthorScenario.CreateAuthor(),
            AuthorScenario.CreateAuthor()
            ];

        _mackAuthorRepository.GetAllAuthorAsync().Returns(Task.FromResult(expectedListAuthor));

        //act
        var result = (await _serviceAuthor.GetAllAuthorAsync()).ToList() ;

        //assert

        result.Should().BeAssignableTo<List<AuthorReadDTO>>();
        result.Should().BeEquivalentTo(expectedListAuthor.Map());
      

    }


    [Fact]
    public async Task Update_ShouldRetrunAuthorUpdated()
    {

        //arrange

        var (dto, author) = AuthorScenario.CreateAuthor_ValidScenarioFormDto();

        _mackAuthorRepository.GetAuthorByIdAsync(Arg.Any<string>())
          .Returns(Task.FromResult(author));

        _mackIUnitOfWork.SaveAsync()
            .Returns(Task.FromResult(true));

        //act


        var result = await _serviceAuthor.UpdateAuthorAsync(dto, author.Id);

        //assert
        result.AsT0.Id.Should();
        result.AsT0.Name.Should().Be(dto.Name);

        await _mackAuthorRepository.Received(1)
            .GetAuthorByIdAsync(Arg.Any<string>());

        await _mackIUnitOfWork
            .Received(1).SaveAsync();
    }

    [Fact]
    public async Task Update_ShouldRetrunAuthorNotFound()
    {

        //arrange
        string idError = Guid.NewGuid().ToString();
        var (dto, _) = (AuthorScenario.CreateAuthor_ValidScenarioFormDto());


        //act
        var result = await _serviceAuthor.UpdateAuthorAsync(dto, idError);

        //assert

         result.AsT1.errors.Should()
            .HaveCount(1);

         await _mackIUnitOfWork
            .DidNotReceive().SaveAsync();
    }



    [Fact]
    public async Task GetById_ShouldRetrunAuthor()
    {

        //arrange
       
        Author author = AuthorScenario.CreateAuthor();

        _mackAuthorRepository.GetAuthorByIdAsync(Arg.Any<string>()).Returns(Task.FromResult(author));

        //act


        var result = await _serviceAuthor.GetAuthorByIdAsync(author.Id);


        //assert

        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(author.Id);
        result.AsT0.Name.Should().Be(author.Name);
        result.AsT0.Post.Should().HaveCount(0);

        await  _mackAuthorRepository.Received(1).GetAuthorByIdAsync(Arg.Any<string>());

    }


    [Fact]
    public async Task GetById_ShouldRetrunNotFound()
    {
        //arrange
       
        string idError = Guid.NewGuid().ToString();

        //act

        var result = await _serviceAuthor.GetAuthorByIdAsync(idError);


        //assert

        result.AsT1.errors.Should().HaveCount(1);
    }



    [Fact]
    public async Task RemoveAuthorById__ShouldRetrunTrue()
    {
        //arrange
        
        Author author = AuthorScenario.CreateAuthor();

        _mackAuthorRepository.GetAuthorByIdAsync(Arg.Any<string>()).Returns(Task.FromResult(author));

        _mackIUnitOfWork.SaveAsync().Returns(Task.FromResult(true));
        //act


        var result = await _serviceAuthor.RemoveAuthorByIdAsync(author.Id);


        //assert

        result.IsT0.Should().BeTrue();
        await _mackAuthorRepository.Received(1)
            .GetAuthorByIdAsync(Arg.Any<string>());

        await _mackIUnitOfWork.Received(1)
            .SaveAsync();


    }

    [Fact]
    public async Task RemoveAuthorById__ShouldRetrunFlase()
    {
        //arrange
        Author author = AuthorScenario.CreateAuthor();
        string isError = Guid.NewGuid().ToString();


        //act


        var result = await _serviceAuthor.RemoveAuthorByIdAsync(isError);


        //assert

        result.AsT1.errors.Should().HaveCount(1);

    }
   




}
