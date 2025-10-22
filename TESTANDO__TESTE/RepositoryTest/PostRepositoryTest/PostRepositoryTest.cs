using Application.Dtos.Models;
using Bogus;
using Domain.Entities;
using Domain.IRepository.IPostRepository;
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

        var author = new Author(expectedAuthouId, new FullName(this._faker.Person.FirstName,""), Guid.NewGuid().ToString());

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

    
    [Fact]
    public async Task GetById__ShouldRetrunNull()
    {

        //arrage
        using var context = new DbContextLite(this._Dbcontext);

        Author author = this._authorBuilder.AuthorEntityBulderPostNULL();
        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);
        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();

        var post = Post.Factory.CreatePost(_faker.Person.UserName, _faker.Lorem.Paragraph(300),  new DateTime(), category.Id, author.Id);

        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();

        var idInvalid = Guid.NewGuid().ToString();

        IPostRepository postRepository = new PostRepository(context);

        //act
        var result = await postRepository.GetById(idInvalid);

        //assert
        result.Should().BeNull();

    }
    [Fact]
    public async Task GetById__ShouldRetrunPost()
    {

        //arrage
        using var context = new DbContextLite(this._Dbcontext);

        Author author = this._authorBuilder.AuthorEntityBulderPostNULL();
        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);
        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();

        var title = _faker.Person.UserName;
        var text = _faker.Lorem.Paragraph(300);
        var data = new DateTime();

        var post = Post.Factory.CreatePost(title, text,  data, category.Id, author.Id);

        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();


        IPostRepository postRepository = new PostRepository(context);

        //act
        var result = await postRepository.GetById(post.Id);

        //assert
        result.AuthorId.Should().Be(author.Id);
        result.CategoryId.Should().Be(category.Id);
        result.Id.Should().Be(result.Id);
        result.Title.Should().Be(title);
        result.Text.Should().Be(text);
        result.Date.Should().Be(data.Date);


    }


    [Fact]
    public async Task DeleteById__ShouldReturnFalse()
    {
       
        //arrage
        using var context = new DbContextLite(this._Dbcontext);

        Author author = this._authorBuilder.AuthorEntityBulderPostNULL();
        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);
        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();

        var post = Post.Factory.CreatePost(_faker.Person.UserName, _faker.Lorem.Paragraph(300), new DateTime(), category.Id, author.Id);

        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();

        var idInvalid = Guid.NewGuid().ToString();

        IPostRepository postRepository = new PostRepository(context);

        //act
        var result = await postRepository.DeleteById(idInvalid);

        //assert
        result.Should().BeFalse();
    }


    [Fact]
    public async Task DeleteById__ShouldRetrunTrue()
    {
        //arrage
        using var context = new DbContextLite(this._Dbcontext);

        Author author = this._authorBuilder.AuthorEntityBulderPostNULL();
        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);
        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();

        var title = _faker.Person.UserName;
        var text = _faker.Lorem.Paragraph(300);
        var data = new DateTime();

        var post = Post.Factory.CreatePost(title, text, data, category.Id, author.Id);

        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();


        IPostRepository postRepository = new PostRepository(context);

        //act
        var result = await postRepository.DeleteById(post.Id);

        //assert
        result.Should().BeTrue();

    }



    [Fact]
    public async Task Update__ShoulRetrunNull()
    {
        //arrage
        using var context = new DbContextLite(this._Dbcontext);

        Author author = this._authorBuilder.AuthorEntityBulderPostNULL();
        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);
        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();
        var idInvalid = Guid.NewGuid().ToString();

        var title = _faker.Person.UserName;
        var text = _faker.Lorem.Paragraph(300);
        var data = new DateTime();

        var post = Post.Factory.CreatePost(
            title,
            text,
            data,
            category.Id,
            author.Id
        );

        var postUpdate = Post.Factory.CreatePost(
            string.Empty,
            string.Empty,
            data, 
            category.Id,
            author.Id
         );

        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();


        IPostRepository postRepository = new PostRepository(context);

        //act
        var result = await postRepository.Update(postUpdate,idInvalid);

        //assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Update__ShouldRetrunUpdatPost()
    {
        //arrage
        using var context = new DbContextLite(this._Dbcontext);

        Author author = this._authorBuilder.AuthorEntityBulderPostNULL();
        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();

        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);
        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();



        var title = _faker.Person.UserName;
        var text = _faker.Lorem.Paragraph(300);
        var data = new DateTime();

        var post = Post.Factory.CreatePost(title,text,data, category.Id, author.Id );
        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();


        PostUpdateDTO postUpdateDTO = new 
        (
            _faker.Person.UserName,
            _faker.Lorem.Paragraph(400),
            string.Empty
        );

       

        post.UpdateAttributes(postUpdateDTO.Title, postUpdateDTO.Text);
        var postUpdate = post;

    
        IPostRepository postRepository = new PostRepository(context);

        //act
        var result = await postRepository.Update(postUpdate, post.Id);

        //assert
        result.Id.Should().Be(post.Id);
        result.Title.Should().Be(postUpdate.Title);
        result.Text.Should().Be(postUpdate.Text);
        result.Date.Should().Be(postUpdate.Date);
        result.CategoryId.Should().Be(postUpdate.CategoryId);
        result.AuthorId.Should().Be(postUpdate.AuthorId);
        

    }

    [Fact]
    public async Task Save__ShouldFalse()
    {
        //arrange
        using var context = new DbContextLite(this._Dbcontext);
        Author author = this._authorBuilder.AuthorEntityBulderPostNULL();
        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();

        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);
        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();



        var title = _faker.Person.UserName;
        var text = _faker.Lorem.Paragraph(300);
        var data = new DateTime();

        var post = Post.Factory.CreatePost(title, text, data, category.Id, author.Id);
        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();

        IPostRepository postRepository = new PostRepository(context);

        //act

        var result = await postRepository.Save();

        //assert
        result.Should().BeFalse();


    }
    [Fact]
    public async Task Save__ShouldTrue()
    {
        //arrange
        using var context = new DbContextLite(this._Dbcontext);
        Author author = this._authorBuilder.AuthorEntityBulderPostNULL();
        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();

        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);
        await context.Category.AddAsync(category);
        await context.SaveChangesAsync();



        var title = _faker.Person.UserName;
        var text = _faker.Lorem.Paragraph(300);
        var data = new DateTime();

        var post = Post.Factory.CreatePost(title, text, data, category.Id, author.Id);
        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();

        IPostRepository postRepository = new PostRepository(context);

        //act
        PostUpdateDTO postUpdateDTO = new
          (
              _faker.Person.UserName,
              _faker.Lorem.Paragraph(400),
              string.Empty
          );

        post.UpdateAttributes(this._faker.Person.FirstName);
        var result = await postRepository.Save();

        //assert
        result.Should().BeTrue();


    }
}
