
using Bogus;
using Domain.Entities;
using Domain.IRepository.IPostRepository;
using Domain.ObjectValues;
using FluentAssertions;
using Infrastructure.Db;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TESTANDO__TESTE.Builder;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.RepositoryTest.CategoryRepositoryTest;

public class CategoryRepositoryTest
{
    private readonly DbContextOptions<DbContextLite> _dbContextOptions;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Faker _faker = new("pt_BR");



    public CategoryRepositoryTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        this._dbContextOptions = new DbContextOptionsBuilder<DbContextLite>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;


    }

    [Fact]
    public async Task CreateCategory_ParamsIsValid_ShouldReturnTrue()
    {
        //arrange
        using var db = new DbContextLite(this._dbContextOptions);

        //act
        CategoryRepository _authorCategory = new(db);
        var category = Category.Factory.CreateCategory(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "Tech");
       await _authorCategory.CreateCategoryAsync(category);

        var result = category;

        //assert
        result.Id.Should().Be(category.Id);
        result.AuthorId.Should().Be(category.AuthorId);
        result.Name.Should().Be(category.Name);
    }

    [Fact]
    public  void GetAllAsync__ShouldReturnListCategory()
    {

        Task.Run(async () =>
        {
            //arrange

            using var context = new DbContextLite(this._dbContextOptions);
            var repo = new CategoryRepository(context);
            var author = new Author(Guid.NewGuid().ToString(), new FullName("ss", "33"), Guid.NewGuid().ToString(), _faker.Person.Email);
            await context.Authors.AddAsync(author);
            await context.SaveChangesAsync();

            var category = Category.Factory.CreateCategory(Guid.NewGuid().ToString(), author.Id, "Tech");
            await context.Category.AddAsync(category);
            await context.SaveChangesAsync();
            var listAuthor = await context.Authors.ToListAsync();
            var listCategory = await context.Category.ToListAsync();

            //act


            var result = await repo.GetAllCategoryAsync();



            //assert
            listAuthor.Should().NotBeEmpty();
            listCategory.Should().NotBeEmpty();
            result.Should().NotBeEmpty();
            result.ToArray()[0].AuthorId.Should().Be(author.Id);
            result.ToArray()[0].Should().Be(category);
        });

       
    }


    [Fact]
    public async Task GetById__ShouldRetrunCategory()
    {

        //arragen
        using var context = new DbContextLite(this._dbContextOptions);
        var categoryRepository = new CategoryRepository(context);

        var author = new Author(Guid.NewGuid().ToString(), new FullName("ss", "33"), Guid.NewGuid().ToString(), _faker.Person.Email);

        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();

        string categoryName = this._faker.Name.Locale;

        var category = Category.Factory.CreateCategory(Guid.NewGuid().ToString(), author.Id, categoryName);

        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();



        //act

        var result = await categoryRepository.GetCategoryByIdAsync(category.Id);


        //assert

        result.Should().NotBeNull();
        result.Author.Should().Be(author);
        result.Name.Should().Be(categoryName);

    } 

    [Fact]
    public async Task GetById__ShouldRetrunNull()
    {

        //arragen
        using var context = new DbContextLite(this._dbContextOptions);
 

        var author = new Author(Guid.NewGuid().ToString(), new FullName("ss", "33"), Guid.NewGuid().ToString(), _faker.Person.Email);

        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();

        string categoryName = this._faker.Name.Locale;

        var category = Category.Factory.CreateCategory(Guid.NewGuid().ToString(), author.Id, categoryName);

        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();

        var idInvalid = Guid.NewGuid().ToString();
        var categoryRepository = new CategoryRepository(context);


        //act

        var result = await categoryRepository.GetCategoryByIdAsync(idInvalid);


        //assert

        result.Should().BeNull();

    } 


    
    [Fact]
    public async Task DeleteById__ShouldRetruntrue()
    {

        //arragen
        using var context = new DbContextLite(this._dbContextOptions);

        var author = new Author(Guid.NewGuid().ToString(), new FullName("ss", "33"), Guid.NewGuid().ToString(), _faker.Person.Email);


        string categoryName = this._faker.Name.Locale;
        var category = Category.Factory.CreateCategory(Guid.NewGuid().ToString(), author.Id, categoryName);
        await context.Authors.AddAsync(author);

        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();

        var categoryRepository = new CategoryRepository(context);
        IUnitOfWork unitOfWork = new UnitOfWork(context);


        //act

        var categoryValid = await categoryRepository.GetCategoryByIdAsync(category.Id);
        categoryRepository.RemoveCategoryAsync(categoryValid!);
        var result = await unitOfWork.SaveAsync(); ;


        //assert

        result.Should().BeTrue();
        (await context.Category.ToListAsync()).Should().HaveCount(0);
        (await context.Authors.ToListAsync()).Should().HaveCount(1);

    }

    [Fact]
    public async Task DeleteById__ShouldRetrunfalse()
    {

        //arragen
        using var context = new DbContextLite(this._dbContextOptions);

        var author = new Author(Guid.NewGuid().ToString(), new FullName("ss", "33"), Guid.NewGuid().ToString(), _faker.Person.Email);

        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();

        string categoryName = this._faker.Name.Locale;

        var category = Category.Factory.CreateCategory(Guid.NewGuid().ToString(), author.Id, categoryName);

        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();

        var idInvalid = Guid.NewGuid().ToString();
        var categoryRepository = new CategoryRepository(context);
        IUnitOfWork unitOfWork = new UnitOfWork(context);


        //act

        var categoryNull = await categoryRepository.GetCategoryByIdAsync(idInvalid);
        if (categoryNull is not null)
        categoryRepository.RemoveCategoryAsync(categoryNull);
        var result = await unitOfWork.SaveAsync() ;


        //assert

        result.Should().BeFalse();
       

    }


    [Fact]
    public async Task Update__ShouldRetrunCategory()
    {
        //arragen
        using var context = new DbContextLite(this._dbContextOptions);

        var author = new Author(Guid.NewGuid().ToString(), new FullName(_faker.Person.FullName, ""), Guid.NewGuid().ToString(), _faker.Person.Email );
   

        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();

        string categoryName = this._faker.Name.Locale;
        string categoryUpdatdName = this._faker.Name.FullName();

        var category = Category.Factory.CreateCategory(Guid.NewGuid().ToString(), author.Id, categoryName);
        var expredtedUpdatedCategory = Category.Factory.CreateCategory(category.Id, author.Id, categoryUpdatdName);

        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();

        var categoryRepository = new CategoryRepository(context);


        //act

        var categoryUPDATE = await categoryRepository.GetCategoryByIdAsync(category.Id);
        categoryUPDATE!.UpdateName(expredtedUpdatedCategory.Name);
        await context.SaveChangesAsync();

        var result = categoryUPDATE;


        //assert

        result.Id.Should().NotBeNull();
        result.Id.Should().Be(category.Id);
        result.Name.Should().Be(expredtedUpdatedCategory.Name);
        result.AuthorId.Should().Be(category.AuthorId);
    

    }
   



}

