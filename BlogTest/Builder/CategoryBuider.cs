using Bogus;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTANDO__TESTE.Builder;

internal class CategoryBuider
{
    private readonly Faker _faker = new("pt_BR");
    public string expectedId { get; set; }
    public string expectedIdAuthor { get; set; }

    public string expectedName { get; set; }



    public Category CategoryEntityBuilder(
        string? expectedId = null, string? expectedIdAuthor = null, string? expectedName=null )
    {

       

        this.expectedId = expectedId ?? Guid.NewGuid().ToString();
        this.expectedIdAuthor = expectedIdAuthor??Guid.NewGuid().ToString();
        this.expectedName = expectedName??  _faker.Music.Locale;

    
        return  Category.Factory.CreateCategory(this.expectedId, this.expectedIdAuthor, this.expectedName);

    }
    public Category CategoryEntityBuilder(string expectedIdAuthor)
    {

        this.expectedIdAuthor = expectedIdAuthor;
        this.expectedName = _faker.Music.Locale;


        return Category.Factory.CreateCategory(this.expectedIdAuthor, this.expectedName);
    }



}
