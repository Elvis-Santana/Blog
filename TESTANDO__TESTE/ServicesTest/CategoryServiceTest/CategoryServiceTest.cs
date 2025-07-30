using Application.Dtos.Models;
using Application.IServices;
using Application.Services.CategoryService;
using Application.Validators.AuthorValidator;
using Bogus;
using Domain.Entities;
using Domain.IRepository.ICategoryRepository;
using FluentAssertions;
using NSubstitute;
using TESTANDO__TESTE.Builder;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.ServicesTest.CategoryServiceTest;

public class CategoryServiceTest
{
    private readonly ICategoryRepository mackCategoryRepository;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Faker _faker = new("pt_BR");
    private readonly CategoryBuider _categoryBuider;
    private readonly AuthorBuilder _authorBuilder;

    public CategoryServiceTest(ITestOutputHelper testOutputHelper)
    {
        this.mackCategoryRepository = Substitute.For<ICategoryRepository>();
        this._testOutputHelper = testOutputHelper;
        this._categoryBuider = new CategoryBuider();
        this._authorBuilder = new AuthorBuilder();
    }

    [Fact]
    public async Task CreateCategory__ParamsIsValid__ShountReturnTrue()
    {
        //arrange
        ICategoryService _CategoryService = new CategoryService(this.mackCategoryRepository, new CategoryValidator());
        AddCategoryInputModel addCategoryInputModel = new(Guid.NewGuid(), _faker.Person.UserName);

        //act
        var result = await _CategoryService.Create(addCategoryInputModel);
        
        //assert
        result.IsT0.Should().BeTrue();

    }

    [Fact]
    public async Task CreateCategory__ParamsIsValid__ShountReturnErros()
    {
        //arrange
        ICategoryService _CategoryService = new CategoryService(this.mackCategoryRepository, new CategoryValidator());
        AddCategoryInputModel addCategoryInputModel = new(Guid.Empty, "");

        //act
        var result = await _CategoryService.Create(addCategoryInputModel);
        
        //assert

        result.Switch(
           e => e.Should().BeFalse(),
           err =>
           {
               err.errors.Count().Should().Be(2);


           }
       );
    }



    [Fact]
    public async Task GetAllAsync__ShountReturnListCategoryViewModel()
    {
        //arrange
        var author = this._authorBuilder.AuthorEntityBulderPostNULL();
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _CategoryService = new CategoryService(this.mackCategoryRepository, new CategoryValidator());
        AddCategoryInputModel addCategoryInputModel = new(Guid.NewGuid(), _faker.Person.UserName);
        this.mackCategoryRepository.GetAsync().Returns(Task.FromResult(new List<Category>() { category }));


        //act
        var result = await _CategoryService.GetAsync();

        //assert
        result.AsT0.Should().NotBeEmpty();

    }


}
