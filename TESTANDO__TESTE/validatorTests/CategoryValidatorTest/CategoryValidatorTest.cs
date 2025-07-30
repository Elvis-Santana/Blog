using Application.Dtos.Models;
using Application.Validators.AuthorValidator;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.validatorTests.CategoryValidatorTest;

public class CategoryValidatorTest
{
    private readonly Faker _faker = new("pt_BR");
    private readonly IValidator<AddCategoryInputModel> _validator;
    private readonly ITestOutputHelper _testOutputHelper;

    public CategoryValidatorTest( ITestOutputHelper testOutputHelper)
    {
        _validator = new CategoryValidator();
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task AddCategoryInputModel_isInvalid_ShouldReturnErros()
    {
        //arrage
        var expectedIdAuthor = Guid.Empty;
        AddCategoryInputModel AddCategoryInputModel = new(expectedIdAuthor, "");

        //act
         var result =await   _validator.TestValidateAsync(AddCategoryInputModel);

        //assert

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(CategoryMsg.CategoryErroNameNotEmpty);

        result.ShouldHaveValidationErrorFor(x => x.IdAuthor)
            .WithErrorMessage(CategoryMsg.CategoryErroIdAuthorNotEmpty);

    }

    [Fact]
    public async Task AddAuthorInputModel_isInvalid_ShouldAddCategoryInputModel()
    {
        //arrage
        AddCategoryInputModel inputModel = null;

        //act
        var result = await _validator.TestValidateAsync(inputModel);

        //assert
        result.ShouldHaveValidationErrorFor(nameof(AddCategoryInputModel)).WithErrorMessage(CategoryMsg.CategoryErroNull);
    

    }

}
