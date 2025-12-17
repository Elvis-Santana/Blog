using Bogus;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTest.Scenario;

internal class CategorySenario
{
    private static readonly Faker _faker = new("pt_BR");



    public static Category CreateCategory() => Category.Factory.CreateCategory(
            Guid.NewGuid().ToString(),
             Guid.NewGuid().ToString(),
              _faker.Music.Locale

    );

    public static Category CreateCategory(string idAuthor)=> Category.Factory.CreateCategory(
           Guid.NewGuid().ToString(),
            idAuthor,
             _faker.Music.Locale

    );






}
