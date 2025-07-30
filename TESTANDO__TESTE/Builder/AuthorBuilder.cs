using Bogus;
using Domain.Entities;
using Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTANDO__TESTE.Builder;

internal  class AuthorBuilder
{
    private   readonly Faker _faker = new("pt_BR");

    public Guid expectedId { get; set; }
    public  FullName expectedName { get; set; }
    public  List<Post> expectedPosts { get; set; }


    public CategoryBuider CategoryBuider;
    public PostBuilder PostBuilder;


   

    public  Author AuthorEntityBulder(FullName ?FullName =null)
    {
         expectedId = Guid.NewGuid();
         expectedName = FullName?? new FullName(this._faker.Person.FullName, this._faker.Person.LastName);


        this.expectedPosts = new List<Post>
        {
            new Post("s","4",new DateTime(),Guid.NewGuid(),Guid.NewGuid())
        };

        return  new(expectedId, expectedName, expectedPosts);
    }

    public Author AuthorEntityBulderPostNULL(FullName? FullName= null)
    {
        this.expectedId = Guid.NewGuid();
        expectedName = FullName ?? new FullName(this._faker.Person.FullName, this._faker.Person.LastName);
        this.expectedPosts = new List<Post>();



        return new(expectedId, expectedName);
    }
   


}
