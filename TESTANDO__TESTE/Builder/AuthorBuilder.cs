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

    public string expectedId { get; set; }
    public  FullName expectedName { get; set; }
    public  List<Post> expectedPosts { get; set; }




   

    public  Author AuthorEntityBulder(FullName ?FullName =null)
    {
         expectedId = Guid.NewGuid().ToString();
         expectedName = FullName?? new FullName(this._faker.Person.FullName, this._faker.Person.LastName);


        this.expectedPosts = new List<Post>
        {
            Post.Factory.CreatePost("s", "4", new DateTime(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
        };

        return  new(expectedId, expectedName, expectedPosts);
    }

    public Author AuthorEntityBulderPostNULL(FullName? FullName= null)
    {
        expectedName = FullName ?? new FullName(this._faker.Person.FullName, this._faker.Person.LastName);
        this.expectedPosts = new List<Post>();

        var result = Author.Factory.CriarAuthor(expectedName);
        this.expectedId = result.Id;

        return result;
    }
   


}
