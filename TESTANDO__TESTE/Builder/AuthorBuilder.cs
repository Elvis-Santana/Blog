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
    public   readonly Faker _faker = new("pt_BR");

    public string? expectedId { get; set; }

    public  FullName? expectedName { get; set; }

    public  List<Post>? expectedPosts { get; set; }

    public string? expectedEmail { get; set; }

    public string? expectedPasswrdHash { get; set; }




    public  Author AuthorEntity( AuthorType authorType )
    {    

        expectedPosts =
        [
            Post.Factory.
            CreatePost("title", "text", new DateTime(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
        ];
      

        expectedId = Guid.NewGuid().ToString();
        expectedName = new (_faker.Person.FirstName, _faker.Person.LastName);
        expectedEmail = _faker.Person.Email;
        expectedPasswrdHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString());


        return authorType switch
        {
            AuthorType.ComPost => new(
                expectedId,
                expectedName,
                expectedPosts,
                expectedPasswrdHash,
                expectedEmail
            ),
            AuthorType.SemPost => new(
                expectedId,
                expectedName,
                expectedPasswrdHash,
                expectedEmail
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(authorType), authorType, null)


        };
      
    }




}

