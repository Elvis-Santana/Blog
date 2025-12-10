using Application.Dtos.Models;
using Application.IServices;
using Application.Services.CategoryService;
using Application.Validators.Validator;
using Application.Validators.Validator.CategoryValidator;
using Bogus;
using Domain.Entities;
using Domain.IRepository.ICategoryRepository;
using FluentAssertions;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using TESTANDO__TESTE.Builder;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.ServicesTest.CategoryServiceTest;

public class CategoryServiceTest
{
    private readonly ICategoryRepository _mackCategoryRepository;
    private readonly IUnitOfWork _mackUnitOfWork;

    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Faker _faker = new("pt_BR");
    private readonly CategoryBuider _categoryBuider;
    private readonly AuthorBuilder _authorBuilder;

    public CategoryServiceTest(ITestOutputHelper testOutputHelper)
    {
        this._mackCategoryRepository = Substitute.For<ICategoryRepository>();
        this._mackUnitOfWork = Substitute.For<IUnitOfWork>();
        this._testOutputHelper = testOutputHelper;
        this._categoryBuider = new CategoryBuider();
        this._authorBuilder = new AuthorBuilder();
    }

    [Fact]
    public async Task CreateCategory__ParamsIsValid__ShountReturnTrue()
    {
        //arrange
        ICategoryService _categoryService = new CategoryService(
              this._mackCategoryRepository,
              new CategoryCreateValidator(),
              new CategoryUpdateValidator(),
               this._mackUnitOfWork
          );

        

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
        ICategoryService _categoryService = new CategoryService(
                  this._mackCategoryRepository,
                  new CategoryCreateValidator(),
                  new CategoryUpdateValidator(),
                   this._mackUnitOfWork
        );
        CategoryCreateDTO addCategoryInputModel = new(string.Empty, "");

        //act
        var result = await _categoryService.CreateCategory(addCategoryInputModel);

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
        var author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService(
               this._mackCategoryRepository,
               new CategoryCreateValidator(),
               new CategoryUpdateValidator(),
                this._mackUnitOfWork
           );
        CategoryCreateDTO addCategoryInputModel = new(Guid.NewGuid().ToString(), _faker.Person.UserName);

        IEnumerable<Category> list = new List<Category>() { category };


        this._mackCategoryRepository.GetAllCategoryAsync().Returns(Task.FromResult(list));


        //act
        var result = (await _categoryService.GetAllCategoryAsync());

        //assert
        result.Should().NotBeEmpty();

    }



    [Fact]
    public async Task DeleteById__ShountReturnTrue()
    {

        //arrage
        var author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService(
            this._mackCategoryRepository,
            new CategoryCreateValidator(),
            new CategoryUpdateValidator(),
             this._mackUnitOfWork
        );

        _mackCategoryRepository.GetCategoryByIdAsync(Arg.Any<string>()).Returns(category);
        _mackUnitOfWork.SaveAsync().Returns(true);



        //act
           
        var result = await _categoryService.RemoveCategoryByIdAsync(category.Id);

        //assert
        result.IsT0.Should().BeTrue();
         this._mackCategoryRepository.Received(1).RemoveCategoryAsync(Arg.Any<Category>());
        await this._mackCategoryRepository.Received(1).GetCategoryByIdAsync(Arg.Any<string>());
        await this._mackUnitOfWork.Received(1).SaveAsync();

    }

    [Fact]
    public async Task DeleteById__ShountReturnErros()
    {

        //arrage
         var author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
         var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService(
             this._mackCategoryRepository,
             new CategoryCreateValidator(),
             new CategoryUpdateValidator(),
              this._mackUnitOfWork
         );

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
        var author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService(
              this._mackCategoryRepository,
              new CategoryCreateValidator(),
              new CategoryUpdateValidator(),
               this._mackUnitOfWork
          );

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

        var author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);
        var catedoryIdInvalid = Guid.NewGuid().ToString();
        ICategoryService _categoryService = new CategoryService(
              this._mackCategoryRepository,
              new CategoryCreateValidator(),
              new CategoryUpdateValidator(),
               this._mackUnitOfWork
        );


        //act
        var result = await _categoryService.GetCategoryByIdAsync(catedoryIdInvalid);

        //assert

        result.AsT1.errors.Should().HaveCount(1);
    }


    [Fact]
    public async Task Update__ShountReturnSucessoCategory()
    {

        //arrage
        var author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService (
            this._mackCategoryRepository,
            new CategoryCreateValidator(),
            new CategoryUpdateValidator(),
             this._mackUnitOfWork
        );


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
        var author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);

        ICategoryService _categoryService = new CategoryService(
            this._mackCategoryRepository,
            new CategoryCreateValidator(),
            new CategoryUpdateValidator(),
             this._mackUnitOfWork
        );

        CategoryUpdateDTO addCategoryInputModel = new(_faker.Random.String2(51));

        this._mackCategoryRepository.GetCategoryByIdAsync(Arg.Any<string>())!.Returns(Task.FromResult(category));

        //act

        var result = await _categoryService.UpdateCategoryAsync(addCategoryInputModel, category.Id);

        //assert

        result.IsT0.Should().BeFalse();
        result.Switch(
             (s) =>{},
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
        var author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        var category = _categoryBuider.CategoryEntityBuilder(author.Id);
        var idInvalid =Guid.NewGuid().ToString();

        ICategoryService _categoryService = new CategoryService(
            this._mackCategoryRepository,
            new CategoryCreateValidator(),
            new CategoryUpdateValidator(),
             this._mackUnitOfWork
        );

        CategoryUpdateDTO addCategoryInputModel = new(_faker.Random.String2(20));


        //act

        var result = await _categoryService.UpdateCategoryAsync(addCategoryInputModel, idInvalid);

        //assert

        result.AsT1.errors.Should().HaveCount(1);
     

    }
   


}