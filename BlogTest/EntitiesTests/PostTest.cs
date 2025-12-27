using BlogTest.Scenario;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.EntitiesTests;

public class PostTest
{
    private readonly Faker _faker = new("pt_BR");
    private readonly ITestOutputHelper _testOutputHelper;

    public PostTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Contructor_DataValid_shouldCreatePostCategory()
    {
        //arrange

       Author expectedAuthor =  AuthorScenario.CreateAuthor();
        Category expectedCategory =  CategorySenario.CreateCategory(expectedAuthor.Id);

        string expectedTitle = this._faker.Music.Locale;
        string expectedText = this._faker.Random.String2(100);
        DateTime expectedDate = DateTime.Now;

        var post = Post.Factory.CreatePost(
            expectedTitle,
            expectedText,
            expectedDate,
            expectedCategory.Id,
            expectedAuthor.Id
        );

        //act



        //assert
        post.Id.Should().NotBeNull();
        post.Title.Should().Be(expectedTitle);
        post.Text.Should().Be(expectedText);
        post.Date.Should().Be(expectedDate);

        post.AuthorId.Should().Be(expectedAuthor.Id);
        post.CategoryId.Should().Be(expectedCategory.Id);
            
       

    }


    [Fact]

    public void Contructor_DataValid_shouldCreatePostCategoryNull()
    {
        Author expectedAuthor = AuthorScenario.CreateAuthor();

        string expectedTitle = this._faker.Music.Locale;
        string expectedText = this._faker.Random.String2(100);
        DateTime expectedDate = DateTime.Now;

        var post = Post.Factory.CreatePost(
            expectedTitle,
            expectedText,
            expectedDate,
            string.Empty,
            expectedAuthor.Id
        );

        //act



        //assert
        post.Id.Should().NotBeNull();
        post.Title.Should().Be(expectedTitle);
        post.Text.Should().Be(expectedText);
        post.Date.Should().Be(expectedDate);

        post.AuthorId.Should().Be(expectedAuthor.Id);
        post.CategoryId.Should().BeNull();



    }
}
