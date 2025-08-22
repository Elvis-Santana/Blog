using Application.Dtos.Models;
using Bogus;
using Domain.Entities;
using Domain.ObjectValues;
using FluentAssertions;
using Mapster;

namespace TESTANDO__TESTE.DtosTests;

public class PostDtoTest
{
    private readonly Faker _faker = new("pt_BR");

    





    [Fact]
    public void Constructor_dataValid_ShouldPostViewModel()
    {
        //arrange
        string expectedTitle = this._faker.Phone.ToString()!;
        string expectedText = this._faker.Lorem.Paragraph(3);
        DateTime expectedDate = this._faker.Date.Recent(30);
        string expectedCategory = this._faker.Person.Company.Name;

        var expectedAuthor = new Author(new FullName(_faker.Person.FirstName,""));
        Post expectedPost = new(expectedTitle, expectedText, expectedDate, new Category(expectedAuthor.Id, expectedCategory), expectedAuthor);
        
        //act

        PostViewModel postDTO = expectedPost.Adapt<PostViewModel>();

        //assert
        postDTO.Id.Should().Be(expectedPost.Id);
        postDTO.Title.Should().Be(expectedTitle);
        postDTO.Text.Should().Be(expectedText);
        postDTO.Date.Should().Be(expectedDate);
        postDTO.Category.Name.Should().Be(expectedCategory);
        postDTO.Category.AuthorId.Should().Be(expectedAuthor.Id);
        postDTO.Id.Should().NotBeEmpty();

        postDTO.AuthorId.Should().Be(expectedAuthor.Id);
    }

    [Fact]
    public void Constructor_dataValid_ShouldPostViewModelCategoryNULL()
    {
        //arrange
        string expectedTitle = this._faker.Phone.ToString()!;
        string expectedText = this._faker.Lorem.Paragraph(3);
        DateTime expectedDate = this._faker.Date.Recent(30);
        string expectedCategory = this._faker.Person.Company.Name;

        var expectedAuthor = new Author(new FullName(_faker.Person.FirstName,""));
        Post expectedPost = new(expectedTitle, expectedText, expectedDate,string.Empty,  expectedAuthor.Id);

     
        //act

        PostViewModel postDTO = expectedPost.Adapt<PostViewModel>();

        //assert
        postDTO.Id.Should().Be(expectedPost.Id);
        postDTO.Title.Should().Be(expectedTitle);
        postDTO.Text.Should().Be(expectedText);
        postDTO.Date.Should().Be(expectedDate);
        postDTO.Id.Should().NotBeEmpty();
        postDTO.CategoryId.Should().BeNull();
        postDTO.Category.Should().BeNull();
        postDTO.AuthorId.Should().Be(expectedAuthor.Id);
    }




    [Fact]
    public void Constructor_dataValid_ShouldAddPostInputModel()
    {

        TypeAdapterConfig<AddPostInputModel, Post>.NewConfig()
          .ConstructUsing(src =>
              new (src.title, src.text, src.date, src.categoryId,src.authorId)
          );

        //Arrange
        string expectedTitle = this._faker.Phone.ToString()!;
        string expectedText = this._faker.Lorem.Paragraph(3);
        DateTime expectedDate = this._faker.Date.Recent(30);
        string expectedCategoryId = Guid.NewGuid().ToString();
        string expectedAuthouId = Guid.NewGuid().ToString();

        var addPostInputModel = new AddPostInputModel
        (
           expectedTitle,
           expectedText,
           expectedDate,
           expectedCategoryId,
           expectedAuthouId
        );
        

        //act

        var result =  addPostInputModel.Adapt<Post>();


        //assert
        result.AuthorId.Should().Be(expectedAuthouId);
        result.CategoryId.Should().Be(expectedCategoryId);
        result.Title.Should().Be(expectedTitle);
        result.Text.Should().Be(expectedText);
        result.Date.Should().Be(expectedDate);
        result.Id.Should().NotBeEmpty();

    }
      [Fact]
    public void Constructor_dataValid_ShouldAddPostInputModelCategoryNULL()
    {

        TypeAdapterConfig<AddPostInputModel, Post>.NewConfig()
          .ConstructUsing(src =>
              new (src.title, src.text, src.date, src.categoryId, src.authorId)
          );

        //Arrange
        string expectedTitle = this._faker.Phone.ToString()!;
        string expectedText = this._faker.Lorem.Paragraph(3);
        DateTime expectedDate = this._faker.Date.Recent(30);
        string expectedAuthouId = Guid.NewGuid().ToString();

        var addPostInputModel = new AddPostInputModel
        (
           expectedTitle,
           expectedText,
           expectedDate,
           "",
           expectedAuthouId
        );


        //act

        var result = addPostInputModel.Adapt<Post>();


        //assert
        result.AuthorId.Should().Be(expectedAuthouId);
        result.CategoryId.Should().BeNull();
        result.Id.Should().NotBeEmpty();

        result.Title.Should().Be(expectedTitle);
        result.Text.Should().Be(expectedText);
        result.Date.Should().Be(expectedDate);

    }


}
