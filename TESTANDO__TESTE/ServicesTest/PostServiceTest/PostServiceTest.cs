using Application.Dtos.Models;
using Application.Services.PostService;
using Application.Validators.Validator;
using Application.Validators.Validator.PostValidator;
using Bogus;
using Domain.Entities;
using Domain.IRepository.IPostRepository;
using Domain.ObjectValues;
using FluentAssertions;
using Mapster;
using NSubstitute;
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
        Author author =  Author.Factory.CriarAuthor(new FullName(_faker.Person.FirstName, ""), Guid.NewGuid().ToString());
        Category category = Category.Factory.CreateCategory(author.Id, _faker.Company.Locale);

        Post post = Post.Factory.CreatePost (
            _faker.Person.FirstName,
            _faker.Person.FirstName,
            new DateTime(),
            category,
            author
        );

        var list = new List<Post>(){ post };
        List<PostReadDTO> expectedCollection = list.Map();

        this._mackServiceAuthor.GetAllPosts().Returns(Task.FromResult(list));
        PostService serviceAuthor = new(this._mackServiceAuthor,new PostCreateValidator(), new PostUpdateValidator());


        //act
         
        var result = await serviceAuthor.GetAll();
        //assert

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<List<PostReadDTO>>();

        for (int i = 0; i < result.Count; i++)
            result[i].Should().BeEquivalentTo(expectedCollection[i]);

        

    }
     [Fact]
    public async Task GetAllPosts__CategoryNULL__ShouldRetrunListPostModelView()
    {
        //arrange
        Author author = Author.Factory.CriarAuthor(new FullName(_faker.Person.FirstName, ""), Guid.NewGuid().ToString());

        Post post = Post.Factory.CreatePost (
            _faker.Person.FirstName,
            _faker.Person.FirstName,
            new DateTime(),
            string.Empty,
            author.Id
        );

        var list = new List<Post>(){ post };
        List<PostReadDTO> expectedCollection = list.Map();

        this._mackServiceAuthor.GetAllPosts().Returns(Task.FromResult(list));
        PostService serviceAuthor = new(this._mackServiceAuthor,new PostCreateValidator(), new PostUpdateValidator());


        //act
         
        var result = await serviceAuthor.GetAll();
        //assert

        result.Should().NotBeNull();
        result[0].AuthorId.Should().Be(author.Id);
        result[0].CategoryId.Should().BeNull();
        result[0].Category.Should().BeNull();
        result.Should().BeAssignableTo<List<PostReadDTO>>();

        for (int i = 0; i < result.Count; i++)
            result[i].Should().BeEquivalentTo(expectedCollection[i]);

        

    }

    [Fact]
    public async Task Create__ShouldRetrunTrue()
    {
        //arrange
        var addPostInputModel = new PostCreateDTO( 
            this._faker.Person.FirstName,
            _faker.Lorem.Paragraph(30),
            new DateTime(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        );

        var expectedPost = (Post)addPostInputModel;
        PostService postService = new(this._mackServiceAuthor, new PostCreateValidator(), new PostUpdateValidator());

        this._mackServiceAuthor.Create(Arg.Any<Post>()).Returns(Task.FromResult(expectedPost));

        //act
        var result = await postService.Create(addPostInputModel);


        //assert

        result.IsT1.Should().BeFalse();
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(expectedPost.Id);
        result.AsT0.Title.Should().Be(expectedPost.Title);
        result.AsT0.Text.Should().Be(expectedPost.Text);
        result.AsT0.Date.Should().Be(expectedPost.Date);
        result.AsT0.CategoryId.Should().Be(expectedPost.CategoryId);
        result.AsT0.AuthorId.Should().Be(expectedPost.AuthorId);
    }
     [Fact]
    public async Task Create__CategoryNULL_ShouldRetrunTrue()
    {
        //arrange
        var addPostInputModel = new PostCreateDTO( 
            this._faker.Person.FirstName,
            _faker.Lorem.Paragraph(30),
            new DateTime(),
            string.Empty,
            Guid.NewGuid().ToString()
        );

        PostService postService = new(
            this._mackServiceAuthor,
            new PostCreateValidator(),
            new PostUpdateValidator()
         );
        var expectedPost = (Post)addPostInputModel;

        this._mackServiceAuthor.Create(Arg.Any<Post>()).Returns(Task.FromResult(expectedPost));

        //act
        var result = await postService.Create(addPostInputModel);


        //assert

        result.IsT1.Should().BeFalse();
        result.IsT0.Should().BeTrue();
        result.AsT0.Id.Should().Be(expectedPost.Id);
        result.AsT0.Text.Should().Be(expectedPost.Text);
        result.AsT0.Title.Should().Be(expectedPost.Title);
        result.AsT0.Date.Should().Be(expectedPost.Date);
        result.AsT0.AuthorId.Should().Be(expectedPost.AuthorId);
        result.AsT0.CategoryId.Should().BeNull();
        result.AsT0.Category.Should().BeNull();

    }

    [Fact]
    public async Task Create__ShouldReturnErrors()
    {
        //arrange
        var addPostInputModel = new PostCreateDTO( 
            this._faker.Person.FirstName,
            _faker.Lorem.Paragraph(2001),
            new DateTime(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        );

        PostService postService = new(
            this._mackServiceAuthor, 
            new PostCreateValidator(),
            new PostUpdateValidator()
         );

        //act
        var result = await postService.Create(addPostInputModel);


        //assert

        result.IsT0.Should().BeFalse();
        result.IsT1.Should().BeTrue();
    }

    [Fact]
    public async Task GetById__ShouldReturnErrors()
    {
        //arrange
        string id = Guid.NewGuid().ToString();

        PostService postService = new(
            this._mackServiceAuthor,
            new PostCreateValidator(),
            new PostUpdateValidator()
         );


        //act
        var result = await postService.GetById(id);

        //assert

        result.AsT1.errors.Should().HaveCount(1);

    }

    [Fact]
    public async Task GetById__ShouldReturnPost()
    {
        //arrange
        string titile = this._faker.Person.FirstName;
        string text =_faker.Text(2000);
        DateTime dateTime = DateTime.Now;
        string authorId = Guid.NewGuid().ToString();

        Post post = Post.Factory.CreatePost(titile, text, dateTime, string.Empty, authorId);


        this._mackServiceAuthor.GetById(post.Id).Returns(Task.FromResult(post));

        PostService postService = new(
            this._mackServiceAuthor,
            new PostCreateValidator(),
            new PostUpdateValidator()
        );


        //act
        PostReadDTO result = ((await postService.GetById(post.Id)).AsT0);

        //assert

        result.Id.Should().Be(post.Id);
        result.Title.Should().Be(titile);
        result.Text.Should().Be(text);
        result.Date.Should().Be(dateTime);
        result.AuthorId.Should().Be(authorId);
        result.CategoryId.Should().Be(null);
        result.Category.Should().Be(null);

    }


    [Fact]
    public async Task DeleteById__ShouldReturnErros()
    {
        //arrange
        string id = Guid.NewGuid().ToString();
        PostService postService = new(
            this._mackServiceAuthor,
            new PostCreateValidator(),
            new PostUpdateValidator()
        );

        //act
        var result = await postService.DeleteById(id);

        //assert
        result.AsT1.errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task DeleteById__ShouldReturnTrue()
    {
        //arrange
        string id = Guid.NewGuid().ToString();

        this._mackServiceAuthor.DeleteById(id).Returns(Task.FromResult(true));
        PostService postService = new(
            this._mackServiceAuthor,
            new PostCreateValidator(),
            new PostUpdateValidator()
        );

        //act
        var result = await postService.DeleteById(id);

        //assert
        result.AsT0.Should().BeTrue();
    }


    [Fact]
    public async Task Update__ShouldReturnErros()
    {
        //arrange
        string id = Guid.NewGuid().ToString();

        PostService postService = new(
            this._mackServiceAuthor,
            new PostCreateValidator(),
            new PostUpdateValidator()
        );

        PostUpdateDTO postUpdatDTO = new
        (
            _faker.Person.UserName,
             _faker.Text(2001),
            string.Empty
        );
        //act
        var result = await postService.Update(postUpdatDTO, id);

        //assert
        result.AsT1.errors.Should().NotBeEmpty();
    }


    [Fact]
    public async Task Update__ShouldReturnPost()
    {
        //arrange

    
        Post post = Post.Factory.CreatePost(
             this._faker.Person.FirstName,
             _faker.Text(40),
              new DateTime(),
              Guid.NewGuid().ToString(),
              Guid.NewGuid().ToString()
        );


        PostUpdateDTO postUpdateDTO = new(
              this._faker.Person.FirstName,
             _faker.Text(40),
              string.Empty

        );

        this._mackServiceAuthor.GetById(Arg.Any<string>()).Returns(Task.FromResult(post));
        post.UpdateAttributes(postUpdateDTO.Title, postUpdateDTO.Text);
        this._mackServiceAuthor.Update(Arg.Any<Post>(), post.Id).Returns(Task.FromResult(post));

        PostService postService = new(this._mackServiceAuthor, new PostCreateValidator(), new PostUpdateValidator());

        //act
        var result = await postService.Update(postUpdateDTO, post.Id);

        //assert
        result.AsT0.Id.Should().Be(post.Id);
        result.AsT0.Text.Should().Be(postUpdateDTO.Text);
        result.AsT0.Title.Should().Be(postUpdateDTO.Title);
        result.AsT0.AuthorId.Should().Be(post.AuthorId);

    }

 

    
}
