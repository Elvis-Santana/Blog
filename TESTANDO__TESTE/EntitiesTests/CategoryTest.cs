using Application.Dtos.Models;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TESTANDO__TESTE.Builder;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.EntitiesTests;

public class CategoryTest
{
    private readonly Faker _faker =new("pt_BR");
    private readonly CategoryBuider buider ;
    private readonly ITestOutputHelper _output;
    public CategoryTest(ITestOutputHelper output)
    {
        this._output = output;
        buider = new CategoryBuider();
    }
    [Fact]
    public void Contructor_DataValid_shouldCategory()
    {
        //arrange
        

        //act
        Category category = buider.CategoryEntityBuilder() ;
        //assert

        category.Id.Should().Be(buider.expectedId);
        category.AuthorId.Should().Be(buider.expectedIdAuthor);
        category.Name.Should().Be(buider.expectedName);
    }
    [Fact]
    public void Contructor_DataValid_shouldCategoryNotPaseId()
    {
        //arrange

        string expecredIdAuthor = Guid.NewGuid().ToString();

        //act
        Category category = buider.CategoryEntityBuilder(expecredIdAuthor);
        //assert

        category.AuthorId.Should().Be(buider.expectedIdAuthor);
        category.Name.Should().Be(buider.expectedName);
        category.Id.Should().NotBeEmpty();

    }

    [Fact]
    public void Contructor_DataValid_shouldCategoryFromAddCategoryInputModel()
    {
        //arrange
        string expecredName = this._faker.Person.FirstName;
        string expecredIdAuthor = Guid.NewGuid().ToString();
        AddCategoryInputModel input = new(expecredIdAuthor, expecredName);

        //act
        var category = (Category)input;
        this._output.WriteLine(category.Id.ToString());

        //assert
        category.Id.Should().NotBe(string.Empty); 
        category.AuthorId.Should().Be(expecredIdAuthor);
        category.Name.Should().Be(expecredName);

    }

    [Fact]
    public void Contructor_DataValid_shouldCategoryViewModel()
    {
        //arrange
        string expecredIdAuthor = Guid.NewGuid().ToString();

        //act
        Category category = buider.CategoryEntityBuilder(expecredIdAuthor);
        CategoryViewModel categoryViewModel = category.Map();

        //assert
        categoryViewModel.AuthorId.Should().Be(expecredIdAuthor);
        categoryViewModel.Name.Should().Be(buider.expectedName);
        categoryViewModel.Id.Should().NotBeEmpty();
    }
}
