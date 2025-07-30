using Bogus;
using Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TESTANDO__TESTE.Builder;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.EntitiesTests;

public class PostTest
{
    private readonly Faker _faker = new("pt_BR");
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly PostBuilder _postBuilder;

    public PostTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _postBuilder = new PostBuilder();
    }

    [Fact]
    public void Contructor_DataValid_shouldCreatePost()
    {
        //arrange
      

        //act
        Post post = _postBuilder.PostEntityBuilderAuthorAndCategory();



        //assert
        post.Id.Should().Be(_postBuilder.expectedId);
        post.Title.Should().Be(_postBuilder.expectedTitle);
        post.Text.Should().Be(_postBuilder.expectedText);
        post.Date.Should().Be(_postBuilder.expectedDate);
        post.Category.Should().Be(_postBuilder.expectedCategory);
        post.Author.Should().Be(_postBuilder.expectedAuthor);
       

    }

}
