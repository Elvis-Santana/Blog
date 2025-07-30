
using Domain.Entities;
using Domain.ObjectValues;
using FluentAssertions;
using Infrastructure.Db;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using TESTANDO__TESTE.Builder;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.RepositoryTest.CategoryRepositoryTest;

public class CategoryRepositoryTest
{
    private readonly DbContextOptions<DbContextLite> _dbContextOptions;
    private readonly ITestOutputHelper _testOutputHelper;

  


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
        var result = await _authorCategory.Create(new (Guid.NewGuid(),Guid.NewGuid(), "Tech"));
        

        //assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllAsync__ShouldReturnListCategory()
    {
        //arrange

        using var context = new DbContextLite(this._dbContextOptions);
        
        var author = new Author(Guid.NewGuid(), new FullName("ss", "33"));
        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();

        var category = new Category(Guid.NewGuid(), author.Id, "Tech");
        await context.Category.AddAsync(category);
        await context.SaveChangesAsync(); 

        //act
        var listAuthor = await context.Authors.ToListAsync();
        var listCategory = await context.Category.ToListAsync();
        var repo = new CategoryRepository(context);
        var result = await repo.GetAsync();



        //assert
        listAuthor.Should().NotBeEmpty();
        listCategory.Should().NotBeEmpty();
        result.Should().NotBeEmpty();
        result.ToArray()[0].IdAuthor.Should().Be(author.Id);
    }
}

