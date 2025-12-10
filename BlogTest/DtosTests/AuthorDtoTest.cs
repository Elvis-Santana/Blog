using Application.Dtos.Models;
using Bogus;
using Domain.Entities;
using Domain.ObjectValues;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TESTANDO__TESTE.Builder;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.DtosTests;

public class AuthorDtoTest
{
    private  readonly Faker _faker = new("pt_BR");
    private readonly ITestOutputHelper _output;
    private readonly AuthorBuilder _authorBuilder;

    public AuthorDtoTest(ITestOutputHelper output)
    {
       this._output = output;
        _authorBuilder = new AuthorBuilder();
    }

    [Fact]
    public void Constructor_dataValid_ShouldCreatAddAuthorCreateDTO()
    {
      

        var authorCreateDTO = new AuthorCreateDTO(new FullName(this._faker.Person.FirstName,""), Guid.NewGuid().ToString(), _faker.Person.Email);


        var author = (Author)authorCreateDTO;


        author.Id.Should().NotBeEmpty();
        author.Name.FirstName.Should().Be(authorCreateDTO.Name.FirstName);
        author.Name.LastName.Should().Be(authorCreateDTO.Name.LastName);
        author.Email.Should().Be(authorCreateDTO.Email);




    }

    [Fact]
    public void Constructor_dataValid_ShouldCreatAuthorReadDTO()
    {
       // arrange
         Author author = _authorBuilder.AuthorEntity(AuthorType.ComPost);

        //act
        AuthorReadDTO authorDto = author.Map();


        // assert
        authorDto.Id.Should()
           .Be(_authorBuilder.expectedId, "Id não é igual a expectedId");

        authorDto.Name.FirstName.Should()
           .Be(_authorBuilder.expectedName.FirstName, "FirstName não é igual a expectedFirstName");

        authorDto.Name.LastName.Should()
            .Be(_authorBuilder.expectedName.LastName, "LastName não é igual a expectedLastName");

        authorDto.Email.Should()
            .Be(_authorBuilder.expectedEmail, "Email não é igual a expectedEmail");

        authorDto.Post.Should()
           .BeEquivalentTo(_authorBuilder.expectedPosts.Select(a => a.Map()).ToList() , "Posts não são equivalentes a expectedIdPosts");

    }
    [Fact]
    public void Constructor_dataValid_ShouldCreatAuthorReadDTOPostNULL()
    {
        //arrange

        Author author = _authorBuilder.AuthorEntity(AuthorType.SemPost);


        //act
        AuthorReadDTO authorDto = author.Map();


        //assert
        authorDto.Id.Should()
            .Be(_authorBuilder.expectedId, "Id não é igual a expectedId");

        authorDto.Name.FirstName.Should()
            .Be(_authorBuilder.expectedName.FirstName, "FirstName não é igual a expectedFirstName");

        authorDto.Name.LastName.Should()
            .Be(_authorBuilder.expectedName.LastName, "LastName não é igual a expectedLastName");

        authorDto.Email.Should()
          .Be(_authorBuilder.expectedEmail, "Email não é igual a expectedEmail");

        authorDto.Post.Should().HaveCount(0) ;

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
