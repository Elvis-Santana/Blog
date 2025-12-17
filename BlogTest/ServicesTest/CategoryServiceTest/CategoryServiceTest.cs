using Application.Dtos.Models;
using Application.IServices;
using Application.Services.CategoryService;
using Application.Validators.Validator.CategoryValidator;
using BlogTest.Scenario;
using Bogus;
using Domain.Entities;
using Domain.IRepository.ICategoryRepository;
using FluentAssertions;
using Infrastructure.UnitOfWork;
using NSubstitute;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.ServicesTest.CategoryServiceTest;

public class CategoryServiceTest
{
    private readonly ICategoryRepository _mackCategoryRepository;
    private readonly IUnitOfWork _mackUnitOfWork;
    private readonly ICategoryService _categoryService;

    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Faker _faker = new("pt_BR");

    public CategoryServiceTest(ITestOutputHelper testOutputHelper)
    {
        this._mackCategoryRepository = Substitute.For<ICategoryRepository>();
        this._mackUnitOfWork = Substitute.For<IUnitOfWork>();
        this._testOutputHelper = testOutputHelper;

        _categoryService = _categoryService = new CategoryService(
                  this._mackCategoryRepository,
                  new CategoryCreateValidator(),
                  new CategoryUpdateValidator(),
                   this._mackUnitOfWork
        );
    }

    [Fact]
    public async Task CreateCategory__ParamsIsValid__ShountReturnTrue()
    {
        //arrange
       
        CategoryCreateDTO addCategoryInputModel = new(Guid.NewGuid().ToString(), _faker.Person.UserName);
        this._mackCategoryRepository.CreateCategoryAsync(Arg.Any<Category>()).Returns(Task.FromResult((Category)addCategoryInputModel));

        //act
        var result = await _categoryService.CreateCategory(addCategoryInputModel);

        //assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().NotBeNull();
        result.AsT0.AuthorId.Should().Be(addCategoryInputModel.AuthorId);
        result.AsT0.Name.Should().Be(addCategoryInputModel.Name);

    }

    [Fact]
    public async Task CreateCategory__ParamsIsValid__ShountReturnErros()
    {
        //arrange
        CategoryCreateDTO addCategoryInputModel = new(string.Empty, "");

        //act
        var result = await _categoryService.CreateCategory(addCategoryInputModel);

        //assert

        result.Switch(
           e => {
           
               e.AuthorId.Should().Be(addCategoryInputModel.AuthorId);
               e.Name.Should().Be(addCategoryInputModel.Name);
           },
           err => err.errors.Count().Should().Be(2)

       );
    }



    [Fact]
    public async Task GetAllAsync__ShountReturnListCategoryViewModel()
    {
        //arrange
        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario.CreateCategory(author.Id);

        CategoryCreateDTO addCategoryInputModel = new(Guid.NewGuid().ToString(), _faker.Person.UserName);

        IEnumerable<Category> list = new List<Category>() { category };
        this._mackCategoryRepository.GetAllCategoryAsync().Returns(Task.FromResult(list));


        //act
        IEnumerable<CategoryReadDTO> result = (await _categoryService.GetAllCategoryAsync());

        //assert
        result.Should().NotBeEmpty();

    }



    [Fact]
    public async Task DeleteById__ShountReturnTrue()
    {
        //arrage;
        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario.CreateCategory(author.Id);

        _mackCategoryRepository.GetCategoryByIdAsync(Arg.Any<string>()).Returns(category);
        _mackUnitOfWork.SaveAsync().Returns(true);

        //act
           
        var result = await _categoryService.RemoveCategoryByIdAsync(category.Id);

        //assert
        result.IsT0.Should()
            .BeTrue();

         this._mackCategoryRepository.Received(1)
            .RemoveCategoryAsync(Arg.Any<Category>());

        await this._mackCategoryRepository.Received(1)
            .GetCategoryByIdAsync(Arg.Any<string>());

        await this._mackUnitOfWork.Received(1)
            .SaveAsync();
    }

    [Fact]
    public async Task DeleteById__ShountReturnErros()
    {
        //arrage
        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario.CreateCategory(author.Id);

        this._mackCategoryRepository.RemoveCategoryAsync(category);

        //act
        var result = await _categoryService.RemoveCategoryByIdAsync(category.Id);

        //assert
        result.AsT1.errors.Should().HaveCount(1);

    }



    [Fact]
    public async Task GetById__ShountReturnCategory()
    {

        //arrage
        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario.CreateCategory(author.Id);

        this._mackCategoryRepository.GetCategoryByIdAsync(category.Id).Returns(Task.FromResult(category));

        //act
           
        var result = await _categoryService.GetCategoryByIdAsync(category.Id);

        //assert
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();


        result.AsT0.AuthorId.Should().Be(category.AuthorId);
        result.AsT0.Id.Should().Be(category.Id);
        result.AsT0.Name.Should().Be(category.Name);

    }

    [Fact]
    public async Task GetById__ShountReturnErros()
    {
        //arrage

        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario.CreateCategory(author.Id);
        var catedoryIdInvalid = Guid.NewGuid().ToString();

   
        //act
        var result = await _categoryService.GetCategoryByIdAsync(catedoryIdInvalid);

        //assert

        result.AsT1.errors.Should().HaveCount(1);
    }


    [Fact]
    public async Task Update__ShountReturnSucessoCategory()
    {

        //arrage
        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario.CreateCategory(author.Id);

      
        CategoryUpdateDTO addCategoryInputModel = new(_faker.Person.UserName);
        this._mackCategoryRepository.GetCategoryByIdAsync(Arg.Any<string>())!.Returns(Task.FromResult(category));

        //act
        var result = await _categoryService.UpdateCategoryAsync(addCategoryInputModel, category.Id);

        //assert
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();



    }


    [Fact]
    public async Task Update__ShountReturnErros()
    {

        //arrage
        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario.CreateCategory(author.Id);

        CategoryUpdateDTO addCategoryInputModel = new(_faker.Random.String2(51));
        this._mackCategoryRepository.GetCategoryByIdAsync(Arg.Any<string>())!.Returns(Task.FromResult(category));

        //act
        var result = await _categoryService.UpdateCategoryAsync(addCategoryInputModel, category.Id);

        //assert
        result.IsT0.Should().BeFalse();
        result.Switch(
             (_) =>{},
             (erro) =>
             {
                 erro.errors.Should().HaveCount(1);
                 erro.errors.ToList().ForEach(x => x.messagem.Should().Be(CategoryMsg.CategoryErroNameMax));
             }

        );

    }
    [Fact]
    public async Task Update__ShountReturnNotFound()
    {
        //arrage

        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario .CreateCategory(author.Id);
        var idInvalid =Guid.NewGuid().ToString();

        CategoryUpdateDTO addCategoryInputModel = new(_faker.Random.String2(20));

        //act
        var result = await _categoryService.UpdateCategoryAsync(addCategoryInputModel, idInvalid);

        //assert
        result.AsT1.errors.Should().HaveCount(1);
     

    }
   


}