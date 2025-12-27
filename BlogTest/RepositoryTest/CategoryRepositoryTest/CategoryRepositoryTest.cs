using Application.IRepository.ICategoryRepository;
using Application.IUnitOfWork;
using BlogTest.Scenario;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Db;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.RepositoryTest.CategoryRepositoryTest;

public class CategoryRepositoryTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Faker _faker = new("pt_BR");
    private readonly DbContextLite _dbContextLite;
    private readonly ICategoryRepository _repository;


    public CategoryRepositoryTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var Options = new DbContextOptionsBuilder<DbContextLite>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

        this._dbContextLite = new(Options);
        this._repository = new CategoryRepository(this._dbContextLite);
    }

    [Fact]
    public async Task CreateCategory_ParamsIsValid_ShouldReturnTrue()
    {
        //arrange

        //act
        var category = CategorySenario.CreateCategory();

       await _repository.CreateCategoryAsync(category);

        var result = category;

        //assert
        result.Id.Should().Be(category.Id);
        result.AuthorId.Should().Be(category.AuthorId);
        result.Name.Should().Be(category.Name);
    }

    [Fact]
    public  async Task GetAllAsync__ShouldReturnListCategory()
    {
        //arrange

        var author = AuthorScenario.CreateAuthor() ;
        var category = CategorySenario.CreateCategory(author.Id);

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.SaveChangesAsync();

      

        //act


        var result = await _repository.GetAllCategoryAsync();



        //assert
        result.Should().HaveCount(1);
        result.ToList()[0].Should().Be(category);

    }


    [Fact]
    public async Task GetById__ShouldRetrunCategory()
    {

        //arragen

        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario.CreateCategory(author.Id);

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.SaveChangesAsync();


        //act

        var result = await _repository.GetCategoryByIdAsync(category.Id);


        //assert

        result.Should().NotBeNull();
        result.Should().Be(category);

    } 

    [Fact]
    public async Task GetById__ShouldRetrunNull()
    {

        //arragen


        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario.CreateCategory(author.Id);

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.SaveChangesAsync();

        var idInvalid = Guid.NewGuid().ToString();


        //act

        var result = await _repository.GetCategoryByIdAsync(idInvalid);


        //assert

        result.Should().BeNull();

    } 


    
    [Fact]
    public async Task DeleteById__ShouldRetruntrue()
    {

        //arragen

        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario.CreateCategory(author.Id);


        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.SaveChangesAsync();

        IUnitOfWork unitOfWork = new UnitOfWork(_dbContextLite);


        //act

        var categoryValid = await _repository.GetCategoryByIdAsync(category.Id);
        _repository.RemoveCategoryAsync(categoryValid!);
        var result = await unitOfWork.SaveAsync(); ;


        //assert

        result.Should().BeTrue();
        (await _dbContextLite.Category.ToListAsync()).Should().HaveCount(0);

    }

    [Fact]
    public async Task DeleteById__ShouldRetrunfalse()
    {

        //arragen

        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario.CreateCategory(author.Id);

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.SaveChangesAsync();

        var idInvalid = Guid.NewGuid().ToString();
        IUnitOfWork unitOfWork = new UnitOfWork(_dbContextLite);


        //act

        var categoryNull = await _repository.GetCategoryByIdAsync(idInvalid);
        if (categoryNull is not null)
            _repository.RemoveCategoryAsync(categoryNull);
        var result = await unitOfWork.SaveAsync() ;


        //assert

        result.Should().BeFalse();
       

    }


    [Fact]
    public async Task Update__ShouldRetrunCategory()
    {
        //arragen

        var author = AuthorScenario.CreateAuthor();
        var category = CategorySenario.CreateCategory(author.Id);

        string categoryUpdatdName = this._faker.Name.FullName();
        var expredtedUpdatedCategory = Category.Factory.CreateCategory(category.Id, author.Id, categoryUpdatdName);

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.SaveChangesAsync();



        //act

        var categoryUPDATE = await _repository.GetCategoryByIdAsync(category.Id);
        categoryUPDATE!.UpdateName(expredtedUpdatedCategory.Name);
        await _dbContextLite.SaveChangesAsync();

        var result = categoryUPDATE;


        //assert

        result.Id.Should().NotBeNull();
        result.Id.Should().Be(category.Id);
        result.Name.Should().Be(expredtedUpdatedCategory.Name);
        result.AuthorId.Should().Be(category.AuthorId);
    

    }
   



}

