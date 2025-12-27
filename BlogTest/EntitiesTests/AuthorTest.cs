using BlogTest.Scenario;
using Bogus;
using Domain.Entities;
using FluentAssertions;

namespace TESTANDO__TESTE.EntitiesTests;

public class AuthorTest
{

    private readonly Faker _faker = new("pt_BR");

  

    [Fact]
    public void Constructor_dataValid_ShouldCreateAuthor()
    {
        var expectedAuthor = AuthorScenario.CreateAuthor();


        // Act
        var author = new  Author(
            expectedAuthor.Id,
            expectedAuthor.Name,
            expectedAuthor.Post,
            expectedAuthor.PasswordHash,
            expectedAuthor.Email
        );

        // Assert
        author.Id.Should().NotBeNullOrEmpty();
        author.Name.Should().Be(expectedAuthor.Name);
        author.PasswordHash.Should().Be(expectedAuthor.PasswordHash);
        author.Email.Should().Be(expectedAuthor.Email);


    }
    [Fact]
    public void Constructor_dataValid_ShouldCreateAuthorLastNameEmpty()
    {
        var expectedAuthor = AuthorScenario.CreateAuthor();
        expectedAuthor.UpdateName(new(expectedAuthor.Name.FirstName, string.Empty));

        // Act
       
        var author = new Author(
          expectedAuthor.Id,
          new(expectedAuthor.Name.FirstName, string.Empty),
          expectedAuthor.Post,
          expectedAuthor.PasswordHash,
          expectedAuthor.Email
        );

        // Assert
        author.Id.Should().NotBeNullOrEmpty();
        author.Name.FirstName.Should().Be(expectedAuthor.Name.FirstName);
        author.Name.LastName.Should().Be(string.Empty);
        author.PasswordHash.Should().Be(expectedAuthor.PasswordHash);
        author.Email.Should().Be(expectedAuthor.Email);

    }


    [Fact]
    public void Constructor_dataValid_ShouldCreateAuthorPostNUll()
    {
        var expectedAuthor= AuthorScenario.CreateAuthor();


        // Act
        var author = Author.CreateAuthor(
            expectedAuthor.Name,
            expectedAuthor.PasswordHash,
            expectedAuthor.Email
        );

        // Assert
        author.Id.Should().NotBeNullOrEmpty();
        author.Name.Should().Be(expectedAuthor.Name);
        author.PasswordHash.Should().Be(expectedAuthor.PasswordHash);
        author.Email.Should().Be(expectedAuthor.Email);
        author.Post.Should().HaveCount(0);


    }

}
