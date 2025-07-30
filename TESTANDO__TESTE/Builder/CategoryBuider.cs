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
    public Guid expectedId { get; set; }
    public Guid expectedIdAuthor { get; set; }

    public string expectedName { get; set; }



    public Category CategoryEntityBuilder(
        Guid? expectedId = null, Guid? expectedIdAuthor = null, string? expectedName=null )
    {

       

        this.expectedId = expectedId ?? Guid.NewGuid();
        this.expectedIdAuthor = expectedIdAuthor??Guid.NewGuid();
        this.expectedName = expectedName??  _faker.Music.Locale;

    
        return new Category(this.expectedId, this.expectedIdAuthor, this.expectedName);

    }
    public Category CategoryEntityBuilder(Guid expectedIdAuthor)
    {

        this.expectedIdAuthor = expectedIdAuthor;
        this.expectedName = _faker.Music.Locale;


        return new Category(this.expectedIdAuthor, this.expectedName);
    }



}
