using Application.Dtos.Models;
using Bogus;
using Domain.Entities;
using Domain.ObjectValues;
using FluentAssertions;
using Infrastructure.Db;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TESTANDO__TESTE.Builder;

namespace TESTANDO__TESTE.RepositoryTest.PostRepositoryTest;

public class PostRepositoryTest
{
    private readonly DbContextOptions<DbContextLite> _Dbcontext;
    private readonly PostBuilder _postBuilder;
    private readonly AuthorBuilder _authorBuilder;
    private readonly CategoryBuider _categoryBuider;
    private readonly Faker _faker = new("pt_BR");


    public PostRepositoryTest()
    {
            this._Dbcontext = new DbContextOptionsBuilder<DbContextLite>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            this._postBuilder = new PostBuilder();
            this._authorBuilder = new AuthorBuilder();
            this._categoryBuider = new CategoryBuider();
    }


    [Fact]
    public async Task CreatePost_ValidsParmas_ShouldRetrunTrue()
    {

        //arragen
       using var context = new DbContextLite(this._Dbcontext);

        Author author = this._authorBuilder.AuthorEntityBulderPostNULL();
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);

        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();
        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();


        var post = this._postBuilder.PostEntityBuilderAuthorAndCategory(category, author);
        PostRepository postRepository = new (context);
        //act

        var result = await postRepository.Create(post);



        //assert
        

        result.AuthorId.Should().Be(author.Id);
        result.Category.Should().Be(category);
        result.Title.Should().Be(this._postBuilder.expectedTitle);
        result.Text.Should().Be(this._postBuilder.expectedText);
        result.Date.Should().Be(this._postBuilder.expectedDate);
        result.Id.Should().Be(this._postBuilder.expectedId);

    }  
    
    [Fact]
    public async Task CreatePost_ValidsParmasCategoryNULL_ShouldRetrunTrue()
    {

        //arragen
       using var context = new DbContextLite(this._Dbcontext);


        string expectedTitle = this._faker.Phone.ToString()!;
        string expectedText = this._faker.Lorem.Paragraph(3);
        DateTime expectedDate = this._faker.Date.Recent(30);
        string expectedAuthouId = Guid.NewGuid().ToString();

        var expectedPost = Post.Factory.CreatePost
        (
           expectedTitle,
           expectedText,
           expectedDate,
           string.Empty,
           expectedAuthouId
        );

        var author = new Author(expectedAuthouId, new FullName(this._faker.Person.FirstName,""));

        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();


        PostRepository postRepository = new (context);
        //act

        var result = await postRepository.Create(expectedPost);



        //assert
        result.AuthorId.Should().Be(author.Id);
        result.Category.Should().BeNull();
        result.Title.Should().Be(expectedTitle);
        result.Text.Should().Be(expectedText);
        result.Date.Should().Be(expectedDate);
        result.Id.Should().Be(expectedPost.Id);






    }

    [Fact]
    public async Task GetAllPosts__ShouldRetrunListPosts()
    {

        //arragen
        using var context = new DbContextLite(this._Dbcontext);

        Author author = this._authorBuilder.AuthorEntityBulderPostNULL();
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);

        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();

        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();

        var post = this._postBuilder.PostEntityBuilderAuthorAndCategory(category, author);

        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync() ;
        PostRepository postRepository = new(context);


        //act

        var result =await  postRepository.GetAllPosts();



        //assert

        result.Should().NotBeEmpty();
      
        result.ForEach(e =>
        {
            e.Text.Should().Be(post.Text);
            e.Title.Should().Be(post.Title);
            e.Date.Should().Be(post.Date);

            e.Author.Should().Be(author);
            e.Category.Should().Be(category);
        });

    }

    


}
