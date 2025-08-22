using Application.Dtos.Models;
using Application.Validators.Validator;
using Bogus;
using Domain.Enums;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTANDO__TESTE.validatorTests.PostValidatorTest;

public class PostValidatorTest
{
    private readonly IValidator<AddPostInputModel> _postValidator;
    private readonly Faker _faker = new("pt_BR");

    public PostValidatorTest()
    {
        this._postValidator = new PostValidator();
    }

    [Fact]

    public async Task AddPostInputModel_isInvalid_ShouldReturnErros1()
    {
        //Arrange

        var addPostInputModel = new AddPostInputModel(
            title: "",
            text: "",
            authorId: string.Empty,
            categoryId: string.Empty,
            date: DateTime.Now
        );


        //act
        var result  =await  _postValidator.TestValidateAsync(addPostInputModel);


        //Assert

        result.IsValid.Should().BeFalse();

        result.ShouldHaveValidationErrorFor(x => x.title)
            .WithErrorMessage(PostMsg.postErroTitleNotEmpty);

        result.ShouldHaveValidationErrorFor(x => x.text)
            .WithErrorMessage(PostMsg.postErroTextNotEmpty);

        result.ShouldHaveValidationErrorFor(x => x.authorId)
            .WithErrorMessage(PostMsg.postErroAuthorIdNotEmpty);

        //result.ShouldHaveValidationErrorFor(x => x.categoryId)
        //    .WithErrorMessage(PostMsg.postErroCategoryIdNotEmpty);


    }   [Fact]

    public async Task AddPostInputModel_isInvalid_ShouldReturnErros2()
    {
        //Arrange

        var addPostInputModel = new AddPostInputModel(
            title: _faker.Lorem.Paragraph(51),
            text: _faker.Lorem.Paragraph(2001),
            authorId:string.Empty,
            categoryId: string.Empty,
            date: DateTime.Now
        );


        //act
        var result  =await  _postValidator.TestValidateAsync(addPostInputModel);


        //Assert

        result.IsValid.Should().BeFalse();

        result.ShouldHaveValidationErrorFor(x => x.title)
            .WithErrorMessage(PostMsg.postErroTitleMax);

        result.ShouldHaveValidationErrorFor(x => x.text)
            .WithErrorMessage(PostMsg.postErroTextMax);

        result.ShouldHaveValidationErrorFor(x => x.authorId)
            .WithErrorMessage(PostMsg.postErroAuthorIdNotEmpty);

        //result.ShouldHaveValidationErrorFor(x => x.categoryId)
        //    .WithErrorMessage(PostMsg.postErroCategoryIdNotEmpty);


    }

 
}
