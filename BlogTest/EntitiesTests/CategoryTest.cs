using Bogus;
using Domain.Entities;
using FluentAssertions;

namespace TESTANDO__TESTE.EntitiesTests;

public class CategoryTest
{
    private readonly Faker _faker =new("pt_BR");
  

   

    [Fact]
    public void Contructor_DataValid_shouldCategory()
    {
        //arrange
        var idAuthor = Guid.NewGuid().ToString();
        var idCategory = Guid.NewGuid().ToString();
        var name = _faker.Music.Locale;

        //act
        Category category =  Category.Factory.CreateCategory
            (
                
                idCategory,
                idAuthor,
                  name

            );
        //assert

        category.Id.Should().Be(idCategory) ;
        category.AuthorId.Should().Be(idAuthor);
        category.Name.Should().Be(name);
    }
   

   
}
