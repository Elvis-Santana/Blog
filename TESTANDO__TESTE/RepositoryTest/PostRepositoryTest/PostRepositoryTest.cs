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
        post.AuthorId.Should().Be(author.Id);
        post.CategoryId.Should().Be(category.Id);
        result.Should().BeTrue();

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
            e.Author.Should().Be(author);
            e.Category.Should().Be(category);
        });

    }
}
