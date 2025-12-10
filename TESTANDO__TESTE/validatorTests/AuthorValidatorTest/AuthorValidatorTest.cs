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
using Application.Dtos.Models;
using Application.Validators.Validator.AuthorValidator;

namespace TESTANDO__TESTE.validatorTests.AuthorValidatorTest;

public class AuthorValidatorTest
{

    private readonly Faker _faker = new("pt_BR");
    private readonly IValidator<AuthorCreateDTO> _validator;
    private readonly ITestOutputHelper _testOutputHelper;

    public AuthorValidatorTest(ITestOutputHelper testOutputHelper)
    {
        _validator = new AuthorCreateValidator();
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task AddAuthorInputModel_isInvalid_ShouldReturnErros()
    {
        //arrage

         FullName fullName = new(string.Empty, _faker.Lorem.Paragraph(101));

         AuthorCreateDTO author_valid = new(fullName, Guid.NewGuid().ToString(), string.Empty);

         AuthorCreateDTO author_valid_email_erro_maximun_length = new(fullName, Guid.NewGuid().ToString(), _faker.Random.String2(256));

         AuthorCreateValidator validator = new AuthorCreateValidator();

        //act
         var result =await _validator.TestValidateAsync(author_valid);


        //assert
        var list = (result.Errors .Select(x => new AppErro(x.ErrorMessage, x.PropertyName))).ToList();
         Errors.CreateError(list).errors.ToList().ForEach(x =>_testOutputHelper.WriteLine(x.ToString()));

        result.ShouldHaveValidationErrorFor(x => x.Name.FirstName).WithErrorMessage(AuthorMsg.FirstNameErroEmpty);
        result.ShouldHaveValidationErrorFor(x => x.Name.LastName).WithErrorMessage(AuthorMsg.LastNameErroMaximumLength);
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage(AuthorMsg.EmailErroEmpty);

        (await _validator.TestValidateAsync(author_valid_email_erro_maximun_length))
            .ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage(AuthorMsg.EmailErroMaximumLength);


    }

 

}
