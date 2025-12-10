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

    private readonly AuthorBuilder _authorBuilder;

    public AuthorTest()
    {
         _authorBuilder = new AuthorBuilder();

    }

    [Fact]
    public void Constructor_dataValid_ShouldCreateAuthor()
    {
        Author _authorValidComPost = _authorBuilder.AuthorEntity(AuthorType.ComPost);

        //Assert
        _authorValidComPost.Id.Should().Be(_authorBuilder.expectedId);

        _authorValidComPost.Name.FirstName.Should()
            .Be(_authorBuilder.expectedName.FirstName, "FirstName não é igual a expectedFirstName");

        _authorValidComPost.Name.LastName.Should()
            .Be(_authorBuilder.expectedName.LastName, "LastName não é igual a expectedLastName");

        _authorValidComPost.Post.Should()
            .BeEquivalentTo(_authorBuilder.expectedPosts, "Posts não são equivalentes a expectedIdPosts");
        _authorValidComPost.Email.Should()
         .Be(_authorBuilder.expectedEmail, "Email não são equivalente a expectedEmail");

    }
    [Fact]
    public void Constructor_dataValid_ShouldCreateAuthorLastNameEmpty()
    {
        Author _authorValidComPost = _authorBuilder.AuthorEntity(AuthorType.ComPost);


        _authorValidComPost.Id.Should()
            .Be(_authorBuilder.expectedId);

        _authorValidComPost.Name.FirstName.Should()
            .Be(_authorBuilder.expectedName.FirstName, "FirstName não é igual a expectedFirstName");

        _authorValidComPost.Name.LastName = string.Empty;
        _authorValidComPost.Name.LastName.Should().BeEmpty();

        _authorValidComPost.Post.Should()
            .BeEquivalentTo(_authorBuilder.expectedPosts, "Posts não são equivalentes a expectedIdPosts");

        _authorValidComPost.Email.Should()
            .Be(_authorBuilder.expectedEmail, "Email não são equivalente a expectedEmail");

    }


    [Fact]
    public void Constructor_dataValid_ShouldCreateAuthorPostNUll()
    {

        Author _authorValidSemPost = _authorBuilder.AuthorEntity(AuthorType.SemPost);

        //assert
        _authorValidSemPost.Id.Should()
            .Be(_authorBuilder.expectedId, "Id não é o experado");

        _authorValidSemPost.Name.FirstName.Should()
            .Be(_authorBuilder.expectedName.FirstName, "FirstName não é igual a expectedFirstName");

        _authorValidSemPost.Name.LastName.Should()
            .Be(_authorBuilder.expectedName.LastName, "LastName não é igual a expectedLastName");

        _authorValidSemPost.Email.Should()
         .Be(_authorBuilder.expectedEmail, "Email não são equivalente a expectedEmail");

        _authorValidSemPost.Post.Should().HaveCount(0,"Posts não é vazio");

    }
  
}
