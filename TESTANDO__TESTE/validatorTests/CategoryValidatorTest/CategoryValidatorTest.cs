using Application.Dtos.Models;
using Application.Validators.AuthorValidator;
using Application.Validators.Validator;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.validatorTests.CategoryValidatorTest;

public class CategoryValidatorTest
{
    private readonly Faker _faker = new("pt_BR");
    private readonly IValidator<AddCategoryInputModel> _addValidator;
    private readonly IValidator<UpdateCategoryInputModel> _updateValidator;

    private readonly ITestOutputHelper _testOutputHelper;

    public CategoryValidatorTest( ITestOutputHelper testOutputHelper)
    {
        _addValidator = new CategoryInputValidator();
        _updateValidator = new CategoryUpdateValidator();
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task AddCategoryInputModel_isInvalid_ShouldReturnErros()
    {
        //arrage
        var expectedIdAuthor = string.Empty;
        AddCategoryInputModel AddCategoryInputModel = new(expectedIdAuthor, "");

        //act
         var result =await   _addValidator.TestValidateAsync(AddCategoryInputModel);

        //assert

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(CategoryMsg.CategoryErroNameNotEmpty);

        result.ShouldHaveValidationErrorFor(x => x.AuthorId)
            .WithErrorMessage(CategoryMsg.CategoryErroIdAuthorNotEmpty);

    }

    [Fact]
    public async Task AddAuthorInputModel_isInvalid_ShouldAddCategoryInputModel()
    {
        //arrage
        AddCategoryInputModel inputModel = null;

        //act
        var result = await _addValidator.TestValidateAsync(inputModel);

        //assert
        result.ShouldHaveValidationErrorFor(nameof(AddCategoryInputModel)).WithErrorMessage(CategoryMsg.CategoryErroNull);
    

    }

    [Fact]
    public async Task UpdateCategoryInputModel_isInvalid_ShouldReturnErros()
    {
        //arrage
        UpdateCategoryInputModel inputModel1 = new ("");
        UpdateCategoryInputModel inputModel2 = new(_faker.Lorem.Paragraph(51));

        //act
        var result1 = await _updateValidator.TestValidateAsync(inputModel1);
        var result2 = await _updateValidator.TestValidateAsync(inputModel2);

        //assert

        result1.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(CategoryMsg.CategoryErroNameNotEmpty);

        result2.ShouldHaveValidationErrorFor(x => x.Name)
          .WithErrorMessage(CategoryMsg.CategoryErroNameMax);

    }


}
