using Application.Dtos;
using Bogus;
using Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace TESTANDO__TESTE;

public class FileDocsTeste
{

    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Faker _faker = new ("pt_BR");
    public FileDocsTeste(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData(TypeFile.PDF,"nameRandom")]
    [InlineData(TypeFile.TXT,"vacalo")]
    [InlineData(TypeFile.DOCX,"Rene")]
    [Trait("Approach", "Theory")]
    public void TheoryCruntructor_Create_File(TypeFile type,string fileName)
    {
        //Arrange
        Guid id = Guid.NewGuid();

        var bytes = Encoding.UTF8.GetBytes("conteudo");
        var stream = new MemoryStream(bytes);
        IFormFile file = new FormFile(stream, 0, stream.Length, "file", $"{fileName}.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };

        //ACt


        var fileDocs = new FileDocs(id, file, type);

        this._testOutputHelper.WriteLine(fileDocs.id.ToString());
        this._testOutputHelper.WriteLine(fileDocs.file.FileName.ToString());
        this._testOutputHelper.WriteLine(fileDocs.type.ToString());


        //Assert

        Assert.Equal(id, fileDocs.id);

        Assert.Equal(type, fileDocs.type);
        Assert.Equal(file.FileName, fileDocs.file.FileName);


    }

    [Fact]
    [Trait("Approach", "NoFaker")]

    public void NoFakerCruntructor_Create_File( )
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var type = TypeFile.PDF;

        var bytes = Encoding.UTF8.GetBytes("conteudo");
        var stream = new MemoryStream(bytes);
        IFormFile file = new FormFile(stream, 0, stream.Length, "file", $"file.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };

        //ACt


        var fileDocs = new FileDocs(id, file, type);

        this._testOutputHelper.WriteLine(fileDocs.id.ToString());
        this._testOutputHelper.WriteLine(fileDocs.file.FileName.ToString());
        this._testOutputHelper.WriteLine(fileDocs.type.ToString());


        //Assert

        Assert.Equal(id, fileDocs.id);

        Assert.Equal(type, fileDocs.type);
        Assert.Equal(file.FileName, fileDocs.file.FileName);


    }

    [Fact]
    [Trait("Approach", " Faker")]

    public void Cruntructor_Create_File()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        var type = TypeFile.PDF;

        var bytes = Encoding.UTF8.GetBytes("conteudo");
        var stream = new MemoryStream(bytes);
        IFormFile file = new FormFile(stream, 0, stream.Length, "file", $"{_faker.Person.FirstName}.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };

        //ACt


        var fileDocs = new FileDocs(id, file, type);

        this._testOutputHelper.WriteLine(fileDocs.id.ToString());
        this._testOutputHelper.WriteLine(fileDocs.file.FileName.ToString());
        this._testOutputHelper.WriteLine(fileDocs.type.ToString());


        //Assert

        Assert.Equal(id, fileDocs.id);

        Assert.Equal(type, fileDocs.type);
        Assert.Equal(file.FileName, fileDocs.file.FileName);


    }

    [Fact]
    [Trait("Approach", " FluentAssertions")]
    public void FluentAssertionsCruntructor_Create_File()
    {
        //Arrange
        Guid expectedId = Guid.NewGuid();
        var expectedType = TypeFile.PDF;

        var bytes = Encoding.UTF8.GetBytes(_faker.Lorem.ToString()!);
        var stream = new MemoryStream(bytes);
        IFormFile expectedFile = new FormFile(stream, 0, stream.Length, "file", $"{_faker.Person.FirstName}.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };
        //ACt


        var fileDocs = new FileDocs(expectedId, expectedFile, expectedType);
        this._testOutputHelper.WriteLine(fileDocs.id.ToString());
        this._testOutputHelper.WriteLine(fileDocs.file.FileName.ToString());
        this._testOutputHelper.WriteLine(fileDocs.type.ToString());


        //Assert
    
        fileDocs.type.Should().Be(expectedType, "type não é igual a expectedType");
        fileDocs.id.Should().Be(expectedId, "id não é igual a expectedId");
        fileDocs.file.FileName.Should().Be(expectedFile.FileName, "file não é igual a expectedFile");


    }

 
}
