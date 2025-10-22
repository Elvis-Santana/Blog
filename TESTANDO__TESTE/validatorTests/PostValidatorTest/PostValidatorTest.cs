using Application.Dtos.Models;
using Application.Validators.Validator.PostValidator;
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
    private readonly IValidator<PostCreateDTO> _postCreateValidator;
    private readonly IValidator<PostUpdateDTO> _postUpdateValidator; 
        
    private readonly Faker _faker = new("pt_BR");

    public PostValidatorTest()
    {
        this._postCreateValidator = new PostCreateValidator();
        this._postUpdateValidator = new PostUpdateValidator();
    }

    [Fact]

    public async Task AddPostInputModel_isInvalid_ShouldReturnErros1()
    {
        //Arrange

        var addPostInputModel = new PostCreateDTO(
            Title: string.Empty,
            Text: string.Empty,
            AuthorId: string.Empty,
            CategoryId: string.Empty,
            Date: DateTime.Now
        );


        //act
        var result  =await  _postCreateValidator.TestValidateAsync(addPostInputModel);


        //Assert

        result.IsValid.Should().BeFalse();

        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(PostMsg.postErroTitleNotEmpty);

        result.ShouldHaveValidationErrorFor(x => x.Text)
            .WithErrorMessage(PostMsg.postErroTextNotEmpty);

        result.ShouldHaveValidationErrorFor(x => x.AuthorId)
            .WithErrorMessage(PostMsg.postErroAuthorIdNotEmpty);

        //result.ShouldHaveValidationErrorFor(x => x.categoryId)
        //    .WithErrorMessage(PostMsg.postErroCategoryIdNotEmpty);


    }   

    [Fact]
    public async Task AddPostInputModel_isInvalid_ShouldReturnErros2()
    {
        //Arrange

        var addPostInputModel = new PostCreateDTO(
            Title: _faker.Text(51),
            Text: _faker.Text(2001),
            AuthorId: string.Empty,
            CategoryId: string.Empty,
            Date: DateTime.Now
        );


        //act
        var result  =await  _postCreateValidator.TestValidateAsync(addPostInputModel);


        //Assert

        result.IsValid.Should().BeFalse();
       
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(PostMsg.postErroTitleMax);

        result.ShouldHaveValidationErrorFor(x => x.Text)
            .WithErrorMessage(PostMsg.postErroTextMax);

        result.ShouldHaveValidationErrorFor(x => x.AuthorId)
            .WithErrorMessage(PostMsg.postErroAuthorIdNotEmpty);

        //result.ShouldHaveValidationErrorFor(x => x.categoryId)
        //    .WithErrorMessage(PostMsg.postErroCategoryIdNotEmpty);


    }

 
}
