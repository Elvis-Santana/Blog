using Application.Dtos;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Text;
using Xunit.Abstractions;

namespace TESTANDO__TESTE;
public class IntegrarTest
{


    private readonly ITestOutputHelper _testOutputHelper;

    public IntegrarTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Cruntructor_Create_requets()
    {
        //Arrange

        var bytes = Encoding.UTF8.GetBytes("conteúdo");
        var stream = new MemoryStream(bytes);
        IFormFile file = new FormFile(stream, 0, stream.Length, "file", "teste.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };
        ;
        
        var expectedId = Guid.NewGuid();
        var expectedFiles = new List<FileDocs>() { new FileDocs(new Guid(), file, TypeFile.PDF) };
        var expecteType = TypeFile.PDF;




        this._testOutputHelper.WriteLine(expectedId.ToString());
        this._testOutputHelper.WriteLine(expecteType.ToString());
        this._testOutputHelper.WriteLine(expectedFiles[0].file.FileName);

        

        //Act
        var interar = new Integrar(expectedId, expectedFiles, expecteType);

        //Assert
        Assert.Equal(expectedId, interar.id);
        Assert.Equal(expecteType, interar.type);

        for (int i = 0; i < expectedFiles.Count(); i++)
        {
           Assert.Equal(expectedFiles[i].file.FileName, interar.files[i].file.FileName);
        }

    }
}
