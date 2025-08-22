using Bogus;
using Domain.Entities;
using Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTANDO__TESTE.Builder;

internal class PostBuilder
{
    private readonly Faker _faker = new("pt_BR");

    public string expectedId { get; set; }
    public string expectedTitle { get; set; }
    public string expectedText { get; set; }
    public DateTime expectedDate { get; set; }
    public Category expectedCategory { get; set; }
    public Author expectedAuthor { get; set; }


    public Post PostEntityBuilderAuthorAndCategory(Category? category=null, Author ?author=null)
    {

        this.expectedTitle = this._faker.Phone.Locale ;
        this.expectedText = this._faker.Lorem.Paragraph(3);
        this.expectedDate = this._faker.Date.Recent(30);

        this.expectedAuthor = author?? new AuthorBuilder().AuthorEntityBulder();

        this.expectedCategory  = category??new CategoryBuider().CategoryEntityBuilder();

        var post = new Post( this.expectedTitle,  this.expectedText,  this.expectedDate,  this.expectedCategory, this.expectedAuthor);
        this.expectedId = post.Id;
        return post;

    }

    public Post PostEntityBuilder(string categoryId, string authorId)
    {

        this.expectedTitle = this._faker.Phone.Locale;
        this.expectedText = this._faker.Lorem.Paragraph(3);
        this.expectedDate = this._faker.Date.Recent(30);

   

        return new(this.expectedTitle, this.expectedText, this.expectedDate, categoryId, authorId); 

    }


}
