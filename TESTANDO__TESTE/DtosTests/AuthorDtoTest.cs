using Application.Dtos.AuthorViewModel;
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
    private readonly Faker _faker = new("pt_BR");
    private readonly ITestOutputHelper _output;
    private readonly AuthorBuilder _authorBuilder;

    public AuthorDtoTest(ITestOutputHelper output)
    {
       this._output = output;
        _authorBuilder = new AuthorBuilder();
    }

    [Fact]
    public void Constructor_dataValid_ShouldCreatAddAuthorInputModel()
    {
       

        var addAuthorInputModel = new AddAuthorInputModel(new FullName(this._faker.Person.FirstName,""));


        var author = addAuthorInputModel.Adapt<Author>();


        author.Id.Should().NotBeEmpty();
        author.Name.FirstName.Should().Be(addAuthorInputModel.Name.FirstName);
        author.Name.LastName.Should().Be(addAuthorInputModel.Name.LastName);




    }
    [Fact]
    public void Constructor_dataValid_ShouldCreatAuthorViewModelPost()
    {
       // arrange
         Author author = _authorBuilder.AuthorEntityBulder();

        //act
        AuthorViewModel authorDto = author.Adapt<AuthorViewModel>();


        // assert
        authorDto.Id.Should()
            .Be(_authorBuilder.expectedId, "Id não é igual a expectedId");

        authorDto.Name.FirstName.Should()
            .Be(_authorBuilder.expectedName.FirstName, "FirstName não é igual a expectedFirstName");

        authorDto.Name.LastName.Should()
            .Be(_authorBuilder.expectedName.LastName, "LastName não é igual a expectedLastName");

        authorDto.Post.Should()
            .BeEquivalentTo(_authorBuilder.expectedPosts.Adapt<List<PostViewModel>>() , "Posts não são equivalentes a expectedIdPosts");

    }
    [Fact]
    public void Constructor_dataValid_ShouldCreatAuthorViewModelPostNULL()
    {
        //arrange

        Author author = _authorBuilder.AuthorEntityBulderPostNULL();


        //act
        AuthorViewModel authorDto = author.Adapt<AuthorViewModel>();


        //assert
        authorDto.Id.Should()
            .Be(_authorBuilder.expectedId, "Id não é igual a expectedId");

        authorDto.Name.FirstName.Should()
            .Be(_authorBuilder.expectedName.FirstName, "FirstName não é igual a expectedFirstName");

        authorDto.Name.LastName.Should()
            .Be(_authorBuilder.expectedName.LastName, "LastName não é igual a expectedLastName");

        authorDto.Post.Should().BeEmpty() ;

    }

    [Fact]
    public void Constructor_dataValid_ShouldCreateAddAuthorInputModel()
    {
        //arrange
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);

        //act
        AddAuthorInputModel authorDTOCreate = new(expectedName);

        //assert
        authorDTOCreate.Name.Should().Be(expectedName);

    }

    
}
