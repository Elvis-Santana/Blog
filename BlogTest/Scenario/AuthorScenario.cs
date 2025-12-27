using Application.Dtos.Models;
using Bogus;
using Domain.Entities;
using Domain.ObjectValues;

namespace BlogTest.Scenario;

public  class AuthorScenario
{
    public  static Faker _faker = new("pt_BR");

 

    public static (AuthorCreateDTO Dto, Author ExpectedAuthor) CreateAuthor_ValidScenarioFormDto( )
    {
        FullName fullName = new (
            _faker.Person.FirstName,
            _faker.Person.LastName
        );

        AuthorCreateDTO dto = new(
            fullName,
            Guid.NewGuid().ToString(),
            _faker.Person.Email
        );

        Author expectedAuthor = Author.CreateAuthor(
            dto.Name,
            dto.Password,
            dto.Email
        );

        return (dto , expectedAuthor);

    }

    public static Author CreateAuthor() => Author.CreateAuthor(
        new FullName(_faker.Person.FirstName, _faker.Person.LastName),
        Guid.NewGuid().ToString(),
        _faker.Person.Email
    );
    
   



   
 
}
