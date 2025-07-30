using Application.Dtos.AuthorViewModel;
using Application.Validators.AuthorValidator;
using Bogus;
using Castle.Core.Resource;
using Domain.ObjectValues;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Domain.Erros.AppErro;
using Xunit.Abstractions;
using Domain.Erros;

namespace TESTANDO__TESTE.validatorTests.AuthorValidatorTest;

public class AuthorValidatorTest
{

    private readonly Faker _faker = new("pt_BR");
    private readonly IValidator<AddAuthorInputModel> _validator;
    private readonly ITestOutputHelper _testOutputHelper;

    public AuthorValidatorTest(ITestOutputHelper testOutputHelper)
    {
        _validator = new AuthorValidator();
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task AddAuthorInputModel_isInvalid_ShouldReturnErros()
    {
        //arrage
            FullName fullName = new(string.Empty, _faker.Lorem.Paragraph(101));
            AddAuthorInputModel inputModel = new(fullName);
            AuthorValidator validator = new AuthorValidator();

        //act
         var result =await _validator.TestValidateAsync(inputModel);


        //assert
        var list = (result.Errors .Select(x => new AppErro(x.ErrorMessage, x.PropertyName))).ToList();
        new Errors(list).errors.ForEach(x =>_testOutputHelper.WriteLine(x.ToString()));

        result.ShouldHaveValidationErrorFor(x => x.Name.FirstName).WithErrorMessage(AuthorMsg.FirstNameErroEmpty);
        result.ShouldHaveValidationErrorFor(x => x.Name.LastName).WithErrorMessage(AuthorMsg.LastNameErroMaximumLength);

    }

    [Fact]
    public async Task  AddAuthorInputModel_isInvalid_ShouldAddAuthorInputModelNull()
    {
        //arrage
        AddAuthorInputModel inputModel = null;

        //act
        var result = await _validator.TestValidateAsync(inputModel);

        //assert
        result.ShouldHaveValidationErrorFor(nameof(AddAuthorInputModel)).WithErrorMessage(AuthorMsg.AuthorErroNull);
        result.IsValid.Should().BeFalse();

    }

}
