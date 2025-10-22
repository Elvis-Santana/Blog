using Application.Dtos.Models;
using Bogus;
using Domain.Entities;
using Domain.ObjectValues;
using FluentAssertions;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TESTANDO__TESTE.Builder;

namespace TESTANDO__TESTE.EntitiesTests;

public class AuthorTest
{

    private readonly Faker _faker = new("pt_BR");


    [Fact]
    public void Constructor_dataValid_ShouldCreateAuthor()
    {
        //arrange
            var authorBuilder = new  AuthorBuilder();
        //act
        Author author = authorBuilder.AuthorEntityBulder();

        //Assert
        author.Id.Should()
            .Be(authorBuilder.expectedId);

        author.Name.FirstName.Should()
            .Be(authorBuilder.expectedName.FirstName, "FirstName não é igual a expectedFirstName");

        author.Name.LastName.Should()
            .Be(authorBuilder.expectedName.LastName, "LastName não é igual a expectedLastName");

        author.Post.Should()
            .BeEquivalentTo(authorBuilder.expectedPosts, "Posts não são equivalentes a expectedIdPosts");

    }
    [Fact]
    public void Constructor_dataValid_ShouldCreateAuthorLastNameEmpty()
    {
        //arrange
        var authorBuilder = new AuthorBuilder();


        //act
        Author author = authorBuilder.AuthorEntityBulder(new FullName(this._faker.Person.FirstName, string.Empty));

        //Assert
        author.Id.Should()
            .Be(authorBuilder.expectedId);

        author.Name.FirstName.Should()
            .Be(authorBuilder.expectedName.FirstName, "FirstName não é igual a expectedFirstName");

        author.Name.LastName.Should().BeEmpty();

        author.Post.Should()
            .BeEquivalentTo(authorBuilder.expectedPosts, "Posts não são equivalentes a expectedIdPosts");

    }


    [Fact]
    public void Constructor_dataValid_ShouldCreateAuthorPostIdNULL()
    {
        //arrange
        var authorBuilder = new AuthorBuilder();


        //act
        Author author = authorBuilder.AuthorEntityBulderPostNULL();


        //assert
        author.Id.Should()
            .Be(authorBuilder.expectedId, "Id não é o experado");

        author.Name.FirstName.Should()
            .Be(authorBuilder.expectedName.FirstName, "FirstName não é igual a expectedFirstName");

        author.Name.LastName.Should()
            .Be(authorBuilder.expectedName.LastName, "LastName não é igual a expectedLastName");
    }
  

    [Fact]
    public void Constructor_dataValid_ShouldCreateAuthorFormAddAuthorInputModelPostNULL()
    {
        //arrange
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);
        AuthorCreateDTO expectedAuthorDTOCreate = new(expectedName,Guid.NewGuid().ToString());

        //act
        Author author = Author.Factory.CriarAuthor(expectedAuthorDTOCreate.Name,Guid.NewGuid().ToString());

        //assert
        author.Name.Should().Be(expectedName);
        author.Id.Should().NotBeEmpty();
    }
    [Fact]
    public void Constructor_dataValid_ShouldCreateAuthorFormAddAuthorInputModelMapperPostNULL()
    {
        //arrange
        FullName expectedName = new(this._faker.Person.FirstName, this._faker.Person.LastName);
        AuthorCreateDTO expectedAuthorInputModel = new(expectedName, Guid.NewGuid().ToString());

        // act
        var author = (Author)expectedAuthorInputModel;

        // assert
        author.Name.Should().BeEquivalentTo(expectedName);
        author.Id.Should().NotBeEmpty();


    }



}
