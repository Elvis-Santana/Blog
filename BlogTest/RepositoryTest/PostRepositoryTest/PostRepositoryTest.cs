using Application.Dtos.Models;
using Application.IRepository.IPostRepository;
using Application.IUnitOfWork;
using BlogTest.Scenario;
using Bogus;
using Domain.Entities;
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

namespace TESTANDO__TESTE.RepositoryTest.PostRepositoryTest;

public class PostRepositoryTest
{
    private readonly Faker _faker = new("pt_BR");

    private readonly DbContextLite _dbContextLite;
    private readonly IPostRepository _repository;


    public PostRepositoryTest()
    {
        var Options = new DbContextOptionsBuilder<DbContextLite>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

     

        this._dbContextLite = new (Options);
        this._repository = new PostRepository(_dbContextLite);
    }


    [Fact]
    public async Task CreatePost_ValidsParmas_ShouldRetrunTrue()
    {
        //arragen
        Author author = AuthorScenario.CreateAuthor();
        Category category = CategorySenario.CreateCategory(author.Id);
        Post post = PostScenario.CreatePostCategory(author.Id, category.Id);

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.SaveChangesAsync();

        //act
        await _repository.CreatePost(post);
        var result = post;
        await _dbContextLite.SaveChangesAsync();

        //assert

        result.Id.Should().Be(post.Id);
        result.AuthorId.Should().Be(author.Id);
        result.CategoryId.Should().Be(category.Id);
        result.Title.Should().Be(post.Title);
        result.Title.Should().Be(post.Title);
        result.Text.Should().Be(post.Text);
  

    }

    [Fact]
    public async Task CreatePost_ValidsParmasCategoryNULL_ShouldRetrunTrue()
    {

        //arragen
        Author author = AuthorScenario.CreateAuthor();
        Post expectedPost = PostScenario.CreatePost(author.Id);




        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.SaveChangesAsync();


        //act

        await _repository.CreatePost(expectedPost);
        await _dbContextLite.SaveChangesAsync();
        var result = expectedPost;



        //assert
        result.AuthorId.Should().Be(author.Id);
        result.Category.Should().BeNull();
        result.Id.Should().Be(expectedPost.Id);
        result.Title.Should().Be(expectedPost.Title);
        result.Text.Should().Be(expectedPost.Text);
        result.Date.Should().Be(expectedPost.Date);

    }

    [Fact]
    public async Task GetAllPosts__ShouldRetrunListPosts()
    {

        //arragen

        Author author = AuthorScenario.CreateAuthor();
        Category category = CategorySenario.CreateCategory(author.Id);
        Post post = PostScenario.CreatePostCategory(author.Id, category.Id);


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

        Author author = AuthorScenario.CreateAuthor();
        Category category = CategorySenario.CreateCategory(author.Id);
        Post post = PostScenario.CreatePostCategory(author.Id, category.Id);

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

        Author author = AuthorScenario.CreateAuthor();
        Category category = CategorySenario.CreateCategory(author.Id);
        Post post = PostScenario.CreatePostCategory(author.Id, category.Id);
   

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
        result.Title.Should().Be(post.Title);
        result.Text.Should().Be(post.Text);
        result.Date.Should().Be(post.Date);


    }


    [Fact]
    public async Task DeleteById__ShouldReturnFalse()
    {

        //arrage

        Author author = AuthorScenario.CreateAuthor();
        Category category = CategorySenario.CreateCategory(author.Id);
        Post post = PostScenario.CreatePostCategory(author.Id, category.Id);



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


        Author author = AuthorScenario.CreateAuthor();
        Category category = CategorySenario.CreateCategory(author.Id);
        Post post = PostScenario.CreatePostCategory(author.Id, category.Id);

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
        var idInvalid = Guid.NewGuid().ToString();

        Author author = AuthorScenario.CreateAuthor();
        Category category = CategorySenario.CreateCategory(author.Id);
        Post post = PostScenario.CreatePostCategory(author.Id, category.Id);

        var postUpdate = Post.Factory.CreatePost(
            string.Empty,
            string.Empty,
            DateTime.Now,
            category.Id,
            author.Id
        );

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.Posts.AddAsync(post);
        await _dbContextLite.SaveChangesAsync();

        //act
        var postById = await _repository.GetPostsById(idInvalid);

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
        Author author = AuthorScenario.CreateAuthor();
        Category category = CategorySenario.CreateCategory(author.Id);
        Post post = PostScenario.CreatePostCategory(author.Id, category.Id);

        await _dbContextLite.Authors.AddAsync(author);
        await _dbContextLite.Category.AddAsync(category);
        await _dbContextLite.Posts.AddAsync(post);
        await _dbContextLite.SaveChangesAsync();


        PostUpdateDTO postUpdateDTO = new 
        (
            _faker.Person.UserName,
            _faker.Lorem.Paragraph(400),
            category.Id
        );

        var postUpdateGet = await _repository.GetPostsById(post.Id);

        postUpdateGet!.UpdateAttributes(postUpdateDTO.Title, postUpdateDTO.Text, postUpdateDTO.CategoryId);


    

        //act
        await _dbContextLite.SaveChangesAsync();

        //assert
        post.Id.Should().Be(postUpdateGet.Id);

        postUpdateGet.Title.Should().Be(postUpdateDTO.Title);
        postUpdateGet.Text.Should().Be(postUpdateDTO.Text);
        postUpdateGet.CategoryId.Should().Be(postUpdateDTO.CategoryId);
        author.Id.Should().Be(postUpdateGet.AuthorId);
        postUpdateGet.Category.Should().Be(category);


    }


}
