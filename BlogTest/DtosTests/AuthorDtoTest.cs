using Application.Dtos.Models;
using BlogTest.Scenario;
using Bogus;
using Domain.Entities;
using Domain.ObjectValues;
using FluentAssertions;
using Mapster;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.DtosTests;

public class AuthorDtoTest
{
    private  readonly Faker _faker = new("pt_BR");
    private readonly ITestOutputHelper _output;

    public AuthorDtoTest(ITestOutputHelper output)
    {
       this._output = output;
    }

    [Fact]
    public void Constructor_dataValid_ShouldCreatAddAuthorCreateDTO()
    {
      

        var authorCreateDTO = new AuthorCreateDTO(new FullName(this._faker.Person.FirstName,""), Guid.NewGuid().ToString(), _faker.Person.Email);


        var author = Author.Factory.CriarAuthor(authorCreateDTO.Name, authorCreateDTO.Password, authorCreateDTO.Email);


        author.Id.Should().NotBeEmpty();
        author.Name.FirstName.Should().Be(authorCreateDTO.Name.FirstName);
        author.Name.LastName.Should().Be(authorCreateDTO.Name.LastName);
        author.Email.Should().Be(authorCreateDTO.Email);




    }

    [Fact]
    public void Constructor_dataValid_ShouldCreatAuthorReadDTO()
    {
        // arrange
        Author author = AuthorScenario.CreateAuthor();

        //act
        AuthorReadDTO authorDto = author.Map();


        // assert
        authorDto.Id.Should()
           .Be(author.Id, "Id não é igual a expectedId");

        authorDto.Name.FirstName.Should()
           .Be(author.Name.FirstName, "FirstName não é igual a expectedFirstName");

        authorDto.Name.LastName.Should()
            .Be(author.Name.LastName, "LastName não é igual a expectedLastName");

        authorDto.Email.Should()
            .Be(author.Email, "Email não é igual a expectedEmail");



    }


    [Fact]
    public void Constructor_dataValid_ShouldCreateAuthorCreateDTO()
    {
        //arrange
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);

        //act
        AuthorCreateDTO authorDTOCreate = new(expectedName, Guid.NewGuid().ToString(), _faker.Person.Email);

        //assert
        authorDTOCreate.Name.Should().Be(expectedName);

    }

   


}
