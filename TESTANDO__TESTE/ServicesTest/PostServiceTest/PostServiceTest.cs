using Application.Dtos.Models;
using Application.IServices;
using Application.Services.PostService;
using Application.Validators.Validator;
using Bogus;
using Domain.Entities;
using Domain.IRepository.IPostRepository;
using Domain.ObjectValues;
using FluentAssertions;
using Mapster;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TESTANDO__TESTE.Builder;

namespace TESTANDO__TESTE.ServicesTest.PostServiceTest;

public class PostServiceTest
{
    private readonly IPostRepository _mackServiceAuthor;
    private readonly PostBuilder postBuilder;
    private readonly Faker _faker = new("pt_BR");

    public PostServiceTest()
    {
        this._mackServiceAuthor = Substitute.For<IPostRepository>();
        this.postBuilder = new PostBuilder();
    }


    [Fact]
    public async Task GetAllPosts__ShouldRetrunListPostModelView()
    {
        //arrange
        Author author = new Author(new FullName(_faker.Person.FirstName, ""));
        Category category = new Category(author.Id, _faker.Company.Locale);

        Post post = new (
            _faker.Person.FirstName,
            _faker.Person.FirstName,
            new DateTime(),
            category,
            author
        );

        var list = new List<Post>(){ post };
        List<PostViewModel> expectedCollection = list.Adapt<List<PostViewModel>>();

        this._mackServiceAuthor.GetAllPosts().Returns(Task.FromResult(list));
        PostService serviceAuthor = new(this._mackServiceAuthor,new PostValidator());


        //act
         
        var result = await serviceAuthor.GetAll();
        //assert

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<List<PostViewModel>>();

        for (int i = 0; i < result.Count; i++)
            result[i].Should().BeEquivalentTo(expectedCollection[i]);

        

    }
       [Fact]
    public async Task GetAllPosts__CategoryNULL__ShouldRetrunListPostModelView()
    {
        //arrange
        Author author = new Author(new FullName(_faker.Person.FirstName, ""));

        Post post = new (
            _faker.Person.FirstName,
            _faker.Person.FirstName,
            new DateTime(),
            string.Empty,
            author.Id
        );

        var list = new List<Post>(){ post };
        List<PostViewModel> expectedCollection = list.Adapt<List<PostViewModel>>();

        this._mackServiceAuthor.GetAllPosts().Returns(Task.FromResult(list));
        PostService serviceAuthor = new(this._mackServiceAuthor,new PostValidator());


        //act
         
        var result = await serviceAuthor.GetAll();
        //assert

        result.Should().NotBeNull();
        result[0].AuthorId.Should().Be(author.Id);
        result[0].CategoryId.Should().BeNull();
        result[0].Category.Should().BeNull();
        result.Should().BeAssignableTo<List<PostViewModel>>();

        for (int i = 0; i < result.Count; i++)
            result[i].Should().BeEquivalentTo(expectedCollection[i]);

        

    }







    [Fact]
    public async Task Create__ShouldRetrunTrue()
    {
        //arrange
        var addPostInputModel = new AddPostInputModel( 
            this._faker.Person.FirstName,
            _faker.Lorem.Paragraph(30),
            new DateTime(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        );

        PostService postService = new(this._mackServiceAuthor, new PostValidator());

        //act
        var result = await postService.Create(addPostInputModel);


        //assert

        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
    }
     [Fact]
    public async Task Create__CategoryNULL_ShouldRetrunTrue()
    {
        //arrange
        var addPostInputModel = new AddPostInputModel( 
            this._faker.Person.FirstName,
            _faker.Lorem.Paragraph(30),
            new DateTime(),
            string.Empty,
            Guid.NewGuid().ToString()
        );

        PostService postService = new(this._mackServiceAuthor, new PostValidator());

        //act
        var result = await postService.Create(addPostInputModel);


        //assert

        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
    }

    [Fact]
    public async Task Create__ShouldReturnErrors()
    {
        //arrange
        var addPostInputModel = new AddPostInputModel( 
            this._faker.Person.FirstName,
            _faker.Lorem.Paragraph(2001),
            new DateTime(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        );

        PostService postService = new(this._mackServiceAuthor, new PostValidator());

        //act
        var result = await postService.Create(addPostInputModel);


        //assert

        result.Switch(
            (s) =>
            {
                s.Should().BeFalse();
            },
            (e) =>
            {
              e.errors.Should().NotBeEmpty();
            }
        );
    }


}
