using Application.Dtos.Models;
using Application.IServices;
using Application.Services.CategoryService;
using Application.Validators.AuthorValidator;
using Application.Validators.Validator;
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
        ICategoryService _categoryService = new CategoryService(
              this.mackCategoryRepository,
              new CategoryInputValidator(),
              new CategoryUpdateValidator()
          );
        AddCategoryInputModel addCategoryInputModel = new(Guid.NewGuid().ToString(), _faker.Person.UserName);

        //act
        var result = await _categoryService.Create(addCategoryInputModel);
        
        //assert
        result.IsT0.Should().BeTrue();

    }

    [Fact]
    public async Task CreateCategory__ParamsIsValid__ShountReturnErros()
    {
        //arrange
        ICategoryService _categoryService = new CategoryService(
                  this.mackCategoryRepository,
                  new CategoryInputValidator(),
                  new CategoryUpdateValidator()
              );
        AddCategoryInputModel addCategoryInputModel = new(string.Empty, "");

        //act
        var result = await _categoryService.Create(addCategoryInputModel);
        
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

        ICategoryService _categoryService = new CategoryService(
               this.mackCategoryRepository,
               new CategoryInputValidator(),
               new CategoryUpdateValidator()
           );
        AddCategoryInputModel addCategoryInputModel = new(Guid.NewGuid().ToString(), _faker.Person.UserName);
        this.mackCategoryRepository.GetAsync().Returns(Task.FromResult(new List<Category>() { category }));


        //act
        var result = await _categoryService.GetAsync();

        //assert
        result.AsT0.Should().NotBeEmpty();

    }



    [Fact]
    public async Task DeleteById__ShountReturnTrue()
    {

        //arrage
        var author = this._authorBuilder.AuthorEntityBulderPostNULL();
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService(
            this.mackCategoryRepository,
            new CategoryInputValidator(),
            new CategoryUpdateValidator()
        );

        this.mackCategoryRepository.DeleteById(category.Id).Returns(Task.FromResult(true));

        //act
           
        var result = await _categoryService.DeleteById(category.Id);

        //assert
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();




    }
    
    [Fact]
    public async Task GetById__ShountReturnCategory()
    {

        //arrage
        var author = this._authorBuilder.AuthorEntityBulderPostNULL();
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService(
              this.mackCategoryRepository,
              new CategoryInputValidator(),
              new CategoryUpdateValidator()
          );

        this.mackCategoryRepository.GetById(category.Id).Returns(Task.FromResult(category));

        //act
           
        var result = await _categoryService.GetById(category.Id);

        //assert
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();




    }

    [Fact]
    public async Task Update__ShountReturnSucessoCategory()
    {

        //arrage
        var author = this._authorBuilder.AuthorEntityBulderPostNULL();
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService (
            this.mackCategoryRepository,
            new CategoryInputValidator(),
            new CategoryUpdateValidator()
        );

        UpdateCategoryInputModel addCategoryInputModel = new(_faker.Person.UserName);
        this.mackCategoryRepository.Update(Arg.Any<Category>(), category.Id)
            .Returns(Task.FromResult(new Category(category.Id, category.AuthorId, addCategoryInputModel.Name)));
        //act

        var result = await _categoryService.Update(addCategoryInputModel, category.Id);

        //assert
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();




    }


    [Fact]
    public async Task Update__ShountReturnErros()
    {

        //arrage
        var author = this._authorBuilder.AuthorEntityBulderPostNULL();
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService(
            this.mackCategoryRepository,
            new CategoryInputValidator(),
            new CategoryUpdateValidator()
        );

        UpdateCategoryInputModel addCategoryInputModel = new(_faker.Lorem.Paragraph(51));


        this.mackCategoryRepository.Update(Arg.Any<Category>(), category.Id)
            .Returns(Task.FromResult(new Category(category.Id, category.AuthorId, addCategoryInputModel.Name)));
        //act

        var result = await _categoryService.Update(addCategoryInputModel, category.Id);

        //assert

        result.IsT0.Should().BeFalse();
        result.Switch(
             (s) =>{},
             (erro) =>
             {
                 erro.errors.Should().HaveCount(1);
                 erro.errors.ForEach(x => x.messagem.Should().Be(CategoryMsg.CategoryErroNameMax));
             }

        );

    }

}