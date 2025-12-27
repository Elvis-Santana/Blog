using Application.Dtos.Models;
using Application.IRepository.IPostRepository;
using Application.IUnitOfWork;
using Application.Services.PostService;
using Application.Validators.Validator.PostValidator;
using Bogus;
using Domain.Entities;
using Domain.ObjectValues;
using FluentAssertions;
using Mapster;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace TESTANDO__TESTE.ServicesTest.PostServiceTest;

public class PostServiceTest
{
    private readonly IPostRepository _mackServiceAuthor;
    private readonly IUnitOfWork _mackSIUnitOfWork;

    private readonly Faker _faker = new("pt_BR");

    public PostServiceTest()
    {
        this._mackServiceAuthor = Substitute.For<IPostRepository>();
        this._mackSIUnitOfWork = Substitute.For<IUnitOfWork>();
    }


    [Fact]
    public async Task GetAllPosts__ShouldRetrunListPostModelView()
    {
        //arrange

        Author author =  Author.Factory.CriarAuthor(new FullName(_faker.Person.FirstName, ""), Guid.NewGuid().ToString(),_faker.Person.Email);
        Category category = Category.Factory.CreateCategory(author.Id, _faker.Company.Locale);

        Post post = Post.Factory.CreatePost (
            _faker.Person.FirstName,
            _faker.Person.FirstName,
            new DateTime(),
            category,
            author
        );

        IEnumerable<Post> list = new List<Post>(){ post };

        IEnumerable<PostReadDTO> expectedCollection = list.Map();

        this._mackServiceAuthor.GetAllPosts().Returns(Task.FromResult(list));
        PostService serviceAuthor = new(this._mackServiceAuthor,new PostCreateValidator(), new PostUpdateValidator(), _mackSIUnitOfWork);


        //act
         
        var result = (await serviceAuthor.GetAllPostsAsync()).ToList();
        //assert

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<List<PostReadDTO>>();

        for (int i = 0; i < result.Count; i++)
            result[i].Should().BeEquivalentTo(expectedCollection.ToList()[i]);

        

    }
     [Fact]
    public async Task GetAllPosts__CategoryNULL__ShouldRetrunListPostModelView()
    {
        //arrange
        Author author = Author.Factory.CriarAuthor(new FullName(_faker.Person.FirstName, ""), Guid.NewGuid().ToString(), _faker.Person.Email);

        Post post = Post.Factory.CreatePost (
            _faker.Person.FirstName,
            _faker.Person.FirstName,
            new DateTime(),
            string.Empty,
            author.Id
        );

        IEnumerable<Post>  list = new List<Post>(){ post };
        IEnumerable<PostReadDTO> expectedCollection = list.Map();

        this._mackServiceAuthor.GetAllPosts().Returns(list);
        PostService serviceAuthor = new(this._mackServiceAuthor,new PostCreateValidator(), new PostUpdateValidator(), _mackSIUnitOfWork);


        //act
         
        var result = (await serviceAuthor.GetAllPostsAsync()).ToList();
        //assert

        result.Should().NotBeNull();
        result[0].AuthorId.Should().Be(author.Id);
        result[0].CategoryId.Should().BeNull();
        result[0].Category.Should().BeNull();
        result.Should().BeAssignableTo<List<PostReadDTO>>();

        for (int i = 0; i < result.Count; i++)
            result[i].Should().BeEquivalentTo(expectedCollection.ToList()[i]);

        

    }

    [Fact]
    public async Task Create__ShouldRetrunTrue()
    {
        //arrange
        var postCreateDTO = new PostCreateDTO( 
            this._faker.Person.FirstName,
            _faker.Lorem.Paragraph(30),
            new DateTime(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        );

        PostService postService = new(this._mackServiceAuthor, new PostCreateValidator(), new PostUpdateValidator(), _mackSIUnitOfWork);

        this._mackServiceAuthor.CreatePost(Arg.Any<Post>()).Returns(Task.FromResult((Post)postCreateDTO));

        //act
        var result = await postService.CreatePostAsync(postCreateDTO);


        //assert

        result.IsT0.Should().BeTrue();
        result.AsT0.Text.Should().Be(postCreateDTO.Text);
        result.AsT0.Title.Should().Be(postCreateDTO.Title);
        result.AsT0.AuthorId.Should().Be(postCreateDTO.AuthorId);
        result.AsT0.CategoryId.Should().Be(postCreateDTO.CategoryId);

        result.AsT0.Id.Should().NotBeEmpty();

        await _mackServiceAuthor.Received(1).CreatePost(Arg.Any<Post>());
        await _mackSIUnitOfWork.Received(1).SaveAsync();
    }
     [Fact]
    public async Task Create__CategoryNULL_ShouldRetrunTrue()
    {
        //arrange
        var postCreateDTO = new PostCreateDTO( 
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
         , _mackSIUnitOfWork);



        this._mackServiceAuthor.CreatePost(Arg.Any<Post>()).Returns(Task.FromResult((Post)postCreateDTO));

        //act
        var result = await postService.CreatePostAsync(postCreateDTO);


        //assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Text.Should().Be(postCreateDTO.Text);
        result.AsT0.Title.Should().Be(postCreateDTO.Title);
        result.AsT0.Id.Should().NotBeEmpty();
        result.AsT0.AuthorId.Should().Be(postCreateDTO.AuthorId);
        result.AsT0.CategoryId.Should().BeNull();

        await _mackServiceAuthor.Received(1).CreatePost(Arg.Any<Post>());
        await _mackSIUnitOfWork.Received(1).SaveAsync();


    }

    [Fact]
    public async Task Create__ShouldReturnErrors()
    {
        //arrange
        var addPostInputModel = new PostCreateDTO( 
            this._faker.Person.FirstName,
            _faker.Random.String2(2001),
            new DateTime(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        );

        PostService postService = new(
            this._mackServiceAuthor, 
            new PostCreateValidator(),
            new PostUpdateValidator()
         , _mackSIUnitOfWork);

        //act
        var result = await postService.CreatePostAsync(addPostInputModel);


        //assert

        result.IsT0.Should().BeFalse();
        await _mackServiceAuthor.Received(0).CreatePost(Arg.Any<Post>());
        await _mackSIUnitOfWork.Received(0).SaveAsync();


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
         , _mackSIUnitOfWork);


        //act
        var result = await postService.GetPostByIdAsync(id);

        //assert

        result.AsT1.errors.Should().HaveCount(1);
        await this._mackServiceAuthor.Received(1).GetPostsById(Arg.Any<string>());

    }

    [Fact]
    public async Task GetById__ShouldReturnPost()
    {
        //arrange
        string titile = this._faker.Person.FirstName;
        string text =_faker.Random.String2(2000);
        DateTime dateTime = DateTime.Now;
        string authorId = Guid.NewGuid().ToString();

        Post post = Post.Factory.CreatePost(titile, text, dateTime, string.Empty, authorId);


        this._mackServiceAuthor.GetPostsById(post.Id)!.Returns(Task.FromResult(post));

        PostService postService = new(
            this._mackServiceAuthor,
            new PostCreateValidator(),
            new PostUpdateValidator()
        , _mackSIUnitOfWork);


        //act
        PostReadDTO result = ((await postService.GetPostByIdAsync(post.Id)).AsT0);

        //assert

        result.Id.Should().Be(post.Id);
        result.Title.Should().Be(titile);
        result.Text.Should().Be(text);
        result.Date.Should().Be(dateTime);
        result.AuthorId.Should().Be(authorId);
        result.CategoryId.Should().Be(null);
        result.Category.Should().Be(null);

        await this._mackServiceAuthor.Received(1).GetPostsById(Arg.Any<string>());

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
        , _mackSIUnitOfWork);

        //act
        var result = await postService.RemovePostByIdAsync(id);

        //assert
        result.AsT1.errors.Should().HaveCount(1);
        await _mackSIUnitOfWork.Received(0).SaveAsync();
    }

    [Fact]
    public async Task DeleteById__ShouldReturnTrue()
    {
        //arrange

        string titile = this._faker.Person.FirstName;
        string text = _faker.Random.String2(2000);
        DateTime dateTime = DateTime.Now;
        string authorId = Guid.NewGuid().ToString();

        Post post = Post.Factory.CreatePost(titile, text, dateTime, string.Empty, authorId);

        this._mackServiceAuthor.RemovePost(Arg.Any<Post>());
        this._mackServiceAuthor.GetPostsById(Arg.Any<string>()).Returns(post);

        this._mackSIUnitOfWork.SaveAsync().Returns(Task.FromResult(true));

        PostService postService = new(
            this._mackServiceAuthor,
            new PostCreateValidator(),
            new PostUpdateValidator()
        , _mackSIUnitOfWork);

        //act
        var result = await postService.RemovePostByIdAsync(post.Id);

        //assert
        result.AsT0.Should().BeTrue();
         _mackServiceAuthor.Received(1).RemovePost(Arg.Any<Post>());
        await _mackSIUnitOfWork.Received(1).SaveAsync();

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
        , _mackSIUnitOfWork);

        PostUpdateDTO postUpdatDTO = new
        (
            _faker.Person.UserName,
             _faker.Random.String2(2001),
            string.Empty
        );
        //act
        var result = await postService.UpdatePostAsync(postUpdatDTO, id);

        //assert
        result.AsT1.errors.Should().NotBeEmpty();
        await this._mackServiceAuthor.Received(0).GetPostsById(Arg.Any<string>());
        await _mackSIUnitOfWork.Received(0).SaveAsync();
    }


    [Fact]
    public async Task Update__ShouldReturnPost()
    {
        //arrange

        Category category = Category.Factory.CreateCategory(
            Guid.NewGuid().ToString(),
            _faker.Person.FullName
        );


        Post post = Post.Factory.CreatePost(
             this._faker.Person.FirstName,
             _faker.Random.String2(40),
              new DateTime(),
              string.Empty,
              Guid.NewGuid().ToString()
        );


        PostUpdateDTO postUpdateDTO = new(
              this._faker.Person.FirstName,
             _faker.Random.String2(40),
             category.Id

        );

        _mackServiceAuthor.GetPostsById(Arg.Any<string>()).Returns(Task.FromResult(post));


        var newPost = post;
        _mackServiceAuthor.When(x =>x.LoadCategoryReferenceAsync(Arg.Any<Post>())).Do(callInfo => {

                   // callInfo.Arg<Post>(0) obtém o primeiro argumento do tipo Post
                   newPost = callInfo.Arg<Post>();

                   // Aqui você poderia, por exemplo, mudar uma propriedade do Post
                   newPost.UpdateAttributes(postUpdateDTO.Title, postUpdateDTO.Text, postUpdateDTO.CategoryId);

                   newPost.UpdateCategoryFromTest(category);
         });

        this._mackSIUnitOfWork.SaveAsync().Returns(Task.FromResult(true));

        PostService postService = new(
            this._mackServiceAuthor, 
            new PostCreateValidator(),
            new PostUpdateValidator(),
            _mackSIUnitOfWork  );

        //act
        var result = await postService.UpdatePostAsync(postUpdateDTO, post.Id);


        //assert
        result.AsT0.Id.Should().Be(post.Id);
        result.AsT0.Text.Should().Be(postUpdateDTO.Text);
        result.AsT0.Title.ToString().Should().Be(postUpdateDTO.Title);
        result.AsT0.CategoryId.Should().Be(postUpdateDTO.CategoryId);
        result.AsT0.Category.Should().Be(category.Map());
        result.AsT0.AuthorId.Should().Be(post.AuthorId);

        await this._mackServiceAuthor.Received(1).GetPostsById(Arg.Any<string>());
        await this._mackServiceAuthor.Received(1).LoadCategoryReferenceAsync(Arg.Any<Post>());
        await _mackSIUnitOfWork.Received(1).SaveAsync();

    }

 

    
}
