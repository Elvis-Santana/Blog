using Application.Dtos.Models;
using Bogus;
using Domain.Entities;
using Domain.IRepository.IPostRepository;
using Domain.ObjectValues;
using FluentAssertions;
using Infrastructure.Db;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
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
    private readonly PostBuilder _postBuilder;
    private readonly AuthorBuilder _authorBuilder;
    private readonly CategoryBuider _categoryBuider;
    private readonly Faker _faker = new("pt_BR");

    private readonly DbContextLite _dbContextLite;
    private readonly IPostRepository _repository;


    public PostRepositoryTest()
    {
        var Options = new DbContextOptionsBuilder<DbContextLite>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        this._postBuilder = new PostBuilder();
        this._authorBuilder = new AuthorBuilder();
        this._categoryBuider = new CategoryBuider();

        this._dbContextLite = new (Options);
        this._repository = new PostRepository(_dbContextLite);
    }


    [Fact]
    public async Task CreatePost_ValidsParmas_ShouldRetrunTrue()
    {
        //arragen
        Author author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.SaveChangesAsync();


        var post = this._postBuilder.PostEntityBuilderAuthorAndCategory(category, author);

        //act
        await _repository.CreatePost(post);
        var result = post;
        await _dbContextLite.SaveChangesAsync();

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


        string expectedTitle = this._faker.Phone.ToString()!;
        string expectedText = this._faker.Lorem.Paragraph(3);
        DateTime expectedDate = this._faker.Date.Recent(30);

       

        var author = _authorBuilder.AuthorEntity(AuthorType.SemPost);
        var expectedPost = Post.Factory.CreatePost
           (
              expectedTitle,
              expectedText,
              expectedDate,
              string.Empty,
              author.Id
           );

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.SaveChangesAsync();


        //act

        await _repository.CreatePost(expectedPost);
        await _dbContextLite.SaveChangesAsync();
        var result = expectedPost;



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

        Author author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);


        var post = this._postBuilder.PostEntityBuilderAuthorAndCategory(category, author);
        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.Posts.AddAsync(post);
        await _dbContextLite.SaveChangesAsync() ;


        //act

        var result =(await _repository.GetAllPosts()).ToList();



        //assert

        result.Should().NotBeEmpty();

        result.ForEach(e =>
        {
            e.Text.Should().Be(post.Text);
            e.Title.Should().Be(post.Title);
            e.Date.Should().Be(post.Date);

            e.Author.Id.Should().Be(author.Id);
            e.Author.Name.Should().Be(author.Name);
            e.Author.Email.Should().Be(author.Email);

            e.CategoryId.Should().Be(category.Id);
            e.Category!.Name.Should().Be(category.Name);
        });
        

    }

    
    [Fact]
    public async Task GetById__ShouldRetrunNull()
    {

        //arrage

        Author author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);

        var post = Post.Factory.CreatePost(_faker.Person.UserName, _faker.Lorem.Paragraph(300),  new DateTime(), category.Id, author.Id);

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.Posts.AddAsync(post);
        await _dbContextLite.SaveChangesAsync();

        var idInvalid = Guid.NewGuid().ToString();


        //act
        var result = await _repository.GetPostsById(idInvalid);

        //assert
        result.Should().BeNull();

    }
    [Fact]
    public async Task GetById__ShouldRetrunPost()
    {

        //arrage

        Author author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);

        var title = _faker.Person.UserName;
        var text = _faker.Lorem.Paragraph(300);
        var data = new DateTime();

        var post = Post.Factory.CreatePost(title, text,  data, category.Id, author.Id);

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.Posts.AddAsync(post);
        await _dbContextLite.SaveChangesAsync();



        //act
        var result = await _repository.GetPostsById(post.Id);

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

        Author author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);

        var post = Post.Factory.CreatePost(_faker.Person.UserName, _faker.Lorem.Paragraph(300), new DateTime(), category.Id, author.Id);


        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.Posts.AddAsync(post);
        await _dbContextLite.SaveChangesAsync();

        var idInvalid = Guid.NewGuid().ToString();

        IUnitOfWork unitOfWork = new UnitOfWork(_dbContextLite);
        //act
        var postNull =  await _repository.GetPostsById(idInvalid);
        if (postNull is not null)
            _repository.RemovePost(postNull);


        
        bool result = await unitOfWork.SaveAsync();


        //assert
        result.Should().BeFalse();


    }


    [Fact]
    public async Task DeleteById__ShouldRetrunTrue()
    {
        //arrage

        Author author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);

        var title = _faker.Person.UserName;
        var text = _faker.Lorem.Paragraph(300);
        var data = new DateTime();

        var post = Post.Factory.CreatePost(title, text, data, category.Id, author.Id);

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.Posts.AddAsync(post);
        await _dbContextLite.SaveChangesAsync();


        IUnitOfWork unitOfWork = new UnitOfWork(_dbContextLite);

        //act
        var postById = await _repository.GetPostsById(post.Id);
        _repository.RemovePost(postById!);
        bool result = await unitOfWork.SaveAsync();

        //assert
        result.Should().BeTrue();

    }



    [Fact]
    public async Task Update__ShoulRetrunNull()
    {
        //arrage

        Author author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);
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

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.Posts.AddAsync(post);
        await _dbContextLite.SaveChangesAsync();

        //act
        var postById = await _repository.GetPostsById(postUpdate.Id);

        if (postById is not null)
            postById.UpdateAttributes(postUpdate.Title, postUpdate.Text);

        
        bool result = await _dbContextLite.SaveChangesAsync() > 0;

        //assert
        result.Should().BeFalse() ;
    }

    [Fact]
    public async Task Update__ShouldRetrunUpdatPost()
    {
        //arrage
        Author author = this._authorBuilder.AuthorEntity(AuthorType.SemPost);
        Category category = this._categoryBuider.CategoryEntityBuilder(_authorBuilder.expectedId);
     

        var title = _faker.Person.UserName;
        var text = _faker.Lorem.Paragraph(300);
        var data = new DateTime();

        var post = Post.Factory.CreatePost(title,text,data, category.Id, author.Id );
        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.Posts.AddAsync(post);
        await _dbContextLite.SaveChangesAsync();


        PostUpdateDTO postUpdateDTO = new 
        (
            _faker.Person.UserName,
            _faker.Lorem.Paragraph(400),
            string.Empty
        );

       

        post.UpdateAttributes(postUpdateDTO.Title, postUpdateDTO.Text);
        var postUpdate = post;

    

        //act
        await _dbContextLite.SaveChangesAsync();

        //assert
        postUpdate.Id.Should().Be(post.Id);
        postUpdate.Title.Should().Be(postUpdate.Title);
        postUpdate.Text.Should().Be(postUpdate.Text);
        postUpdate .Date.Should().Be(postUpdate.Date);
        postUpdate.CategoryId.Should().Be(postUpdate.CategoryId);
        postUpdate.AuthorId.Should().Be(postUpdate.AuthorId);
        

    }

   
}
