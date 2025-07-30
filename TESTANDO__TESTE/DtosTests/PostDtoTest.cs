using Application.Dtos.AuthorViewModel;
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

public class PostDtoTest
{
    private readonly Faker _faker = new("pt_BR");

    [Fact]
    public void Constructor_dataValid_ShouldCreatePostDto()
    {
        //arrange
        Guid expectedId = Guid.NewGuid();
        string expectedTitle = this._faker.Phone.ToString()!;
        string expectedText = this._faker.Lorem.Paragraph(3);
        DateTime expectedDate = this._faker.Date.Recent(30);
        string expectedCategory = this._faker.Person.Company.Name;
        Guid expectedIdAuthor = Guid.NewGuid();


        //act
        //Post post = new(expectedId, expectedTitle, expectedText, expectedDate, new Category(expectedIdAuthor, expectedCategory), expectedIdAuthor);

        //PostViewModel postDTO = post.Adapt<PostViewModel>();

        ////assert
        //postDTO.Id.Should().Be(expectedId);
        //postDTO.Title.Should().Be(expectedTitle);
        //postDTO.Text.Should().Be(expectedText);
        //postDTO.Date.Should().Be(expectedDate);
        //postDTO.Category.Name.Should().Be(expectedCategory);
        //postDTO.Category.IdAuthor.Should().Be(expectedIdAuthor);
        //postDTO.Id.Should().NotBeEmpty();

        //postDTO.AuthorId.Should().Be(expectedIdAuthor);
    }

  
}
