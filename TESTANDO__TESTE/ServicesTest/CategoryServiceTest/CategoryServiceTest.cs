using Application.Dtos.Models;
using Application.IServices;
using Application.Services.CategoryService;
using Application.Validators.Validator;
using Application.Validators.Validator.CategoryValidator;
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
              new CategoryCreateValidator(),
              new CategoryUpdateValidator()
          );

        

        CategoryCreateDTO addCategoryInputModel = new(Guid.NewGuid().ToString(), _faker.Person.UserName);
        var expectedCategory = (Category)addCategoryInputModel;

        this.mackCategoryRepository.Create(Arg.Any<Category>()).Returns(Task.FromResult(expectedCategory));

        //act
        var result = await _categoryService.Create(addCategoryInputModel);
        
        //assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(expectedCategory.Id);
        result.AsT0.AuthorId.Should().Be(expectedCategory.AuthorId);
        result.AsT0.Name.Should().Be(expectedCategory.Name);

    }

    [Fact]
    public async Task CreateCategory__ParamsIsValid__ShountReturnErros()
    {
        //arrange
        ICategoryService _categoryService = new CategoryService(
                  this.mackCategoryRepository,
                  new CategoryCreateValidator(),
                  new CategoryUpdateValidator()
        );
        CategoryCreateDTO addCategoryInputModel = new(string.Empty, "");

        //act
        var result = await _categoryService.Create(addCategoryInputModel);

        //assert

        result.Switch(
           e => {
           
               e.AuthorId.Should().Be(addCategoryInputModel.AuthorId);
               e.Name.Should().Be(addCategoryInputModel.Name);
           },
           err =>

               err.errors.Count().Should().Be(2)

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
               new CategoryCreateValidator(),
               new CategoryUpdateValidator()
           );
        CategoryCreateDTO addCategoryInputModel = new(Guid.NewGuid().ToString(), _faker.Person.UserName);
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
            new CategoryCreateValidator(),
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
    public async Task DeleteById__ShountReturnErros()
    {

        //arrage
         var author = this._authorBuilder.AuthorEntityBulderPostNULL();
         var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService(
             this.mackCategoryRepository,
             new CategoryCreateValidator(),
             new CategoryUpdateValidator()
         );

        this.mackCategoryRepository.DeleteById(category.Id).Returns(Task.FromResult(false));

        //act
        var result = await _categoryService.DeleteById(category.Id);


        //assert
        result.AsT1.errors.Should().HaveCount(1);

    }



    [Fact]
    public async Task GetById__ShountReturnCategory()
    {

        //arrage
        var author = this._authorBuilder.AuthorEntityBulderPostNULL();
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService(
              this.mackCategoryRepository,
              new CategoryCreateValidator(),
              new CategoryUpdateValidator()
          );

        this.mackCategoryRepository.GetById(category.Id).Returns(Task.FromResult(category));

        //act
           
        var result = await _categoryService.GetById(category.Id);

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

        var author = this._authorBuilder.AuthorEntityBulderPostNULL();
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);
        var catedoryIdInvalid = Guid.NewGuid().ToString();
        ICategoryService _categoryService = new CategoryService(
              this.mackCategoryRepository,
              new CategoryCreateValidator(),
              new CategoryUpdateValidator()
        );


        //act
        var result = await _categoryService.GetById(catedoryIdInvalid);

        //assert

        result.AsT1.errors.Should().HaveCount(1);
    }



    [Fact]
    public async Task Update__ShountReturnSucessoCategory()
    {

        //arrage
        var author = this._authorBuilder.AuthorEntityBulderPostNULL();
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService (
            this.mackCategoryRepository,
            new CategoryCreateValidator(),
            new CategoryUpdateValidator()
        );


        CategoryUpdateDTO addCategoryInputModel = new(_faker.Person.UserName);
        this.mackCategoryRepository.GetById(Arg.Any<string>()).Returns(Task.FromResult(category));


        this.mackCategoryRepository.Update(Arg.Any<Category>(), category.Id).Returns(Task.Run(() =>
        {
            category.UpdateName(addCategoryInputModel.Name);
            return category;
        }));

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
            new CategoryCreateValidator(),
            new CategoryUpdateValidator()
        );

        CategoryUpdateDTO addCategoryInputModel = new(_faker.Lorem.Paragraph(51));


        this.mackCategoryRepository.Update(Arg.Any<Category>(), category.Id)
            .Returns(Task.FromResult(Category.Factory.CreateCategory(category.Id, category.AuthorId, addCategoryInputModel.Name)));
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
    [Fact]
    public async Task Update__ShountReturnNotFound()
    {

        //arrage
        var author = this._authorBuilder.AuthorEntityBulderPostNULL();
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);
        var idInvalid =Guid.NewGuid().ToString();

        ICategoryService _categoryService = new CategoryService(
            this.mackCategoryRepository,
            new CategoryCreateValidator(),
            new CategoryUpdateValidator()
        );

        CategoryUpdateDTO addCategoryInputModel = new(_faker.Lorem.Paragraph(20));


        //act

        var result = await _categoryService.Update(addCategoryInputModel, idInvalid);

        //assert

        result.AsT1.errors.Should().HaveCount(1);
     

    }

}