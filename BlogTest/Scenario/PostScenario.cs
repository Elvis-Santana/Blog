using Bogus;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTest.Scenario;

internal static class PostScenario
{
    private static readonly Faker _faker = new("pt_BR");

    public static Post CreatePost(string idAuthor)
    {

        return Post.Factory.CreatePost(
            _faker.Music.Locale,
            _faker.Random.String2(100),
            new DateTime(),
            string.Empty,
            idAuthor
        );

    }

    public static Post CreatePostCategory(string idAuthor,string idCategory)
    {
        return Post.Factory.CreatePost(
            _faker.Music.Locale,
            _faker.Random.String2(100),
            new DateTime(),
            idCategory,
            idAuthor
        );

    }


}
