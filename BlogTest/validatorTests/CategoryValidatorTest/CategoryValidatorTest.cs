using Application.Dtos.Models;
using Application.Validators.Validator;
using Application.Validators.Validator.CategoryValidator;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit.Abstractions;

namespace TESTANDO__TESTE.validatorTests.CategoryValidatorTest;

public class CategoryValidatorTest
{
    private readonly Faker _faker = new("pt_BR");
    private readonly IValidator<CategoryCreateDTO> _addValidator;
    private readonly IValidator<CategoryUpdateDTO> _updateValidator;

    private readonly ITestOutputHelper _testOutputHelper;

    public CategoryValidatorTest( ITestOutputHelper testOutputHelper)
    {
        _addValidator = new CategoryCreateValidator();
        _updateValidator = new CategoryUpdateValidator();
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task AddCategoryInputModel_isInvalid_ShouldReturnErros()
    {
        //arrage
        var expectedIdAuthor = string.Empty;
        CategoryCreateDTO AddCategoryInputModel = new(expectedIdAuthor, "");

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
        CategoryCreateDTO inputModel = null;

        //act
        var result = await _addValidator.TestValidateAsync(inputModel);

        //assert
        result.ShouldHaveValidationErrorFor(nameof(CategoryCreateDTO)).WithErrorMessage(CategoryMsg.CategoryErroNull);
    

    }

    [Fact]
    public async Task UpdateCategoryInputModel_isInvalid_ShouldReturnErros()
    {
        //arrage
        CategoryUpdateDTO inputModel1 = new ("");
        CategoryUpdateDTO inputModel2 = new(_faker.Lorem.Paragraph(51));

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
