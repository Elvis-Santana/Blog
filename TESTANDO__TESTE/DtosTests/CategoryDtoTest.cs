using Application.Dtos.Models;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTANDO__TESTE.DtosTests;

public class CategoryDtoTest
{
    private readonly Faker _faker = new("pt_BR");

    [Fact]
    public void Category__dataValid_Create()
    {
        //arrange
        var expectedIdAuthor = Guid.NewGuid().ToString();
        var exprectedName = _faker.Person.LastName;
        UpdateCategoryInputModel updateCategory = new (exprectedName);

        //Category category = new Category(expectedIdAuthor, _faker.Person.UserName);

        //act

        var result = updateCategory.Adapt<Category>();


        //assert
        result.Name.Should().NotBeEmpty();
        result.Name.Should().Be(exprectedName);
    }
}
