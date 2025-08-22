using Application.Dtos.Models;
using Application.IServices;
using Application.Validators.AuthorValidator;
using Domain.Entities;
using Domain.Erros;
using Domain.Erros.AppErro;
using Domain.IRepository.IAuthorRepository;
using FluentValidation;
using Mapster;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AuthorService;

public class AuthorService(IAuthorRepository authorRepository, IValidator<AddAuthorInputModel> validator) : IServiceAuthor
{
    private readonly IAuthorRepository _authorRepository = authorRepository;
    private readonly IValidator<AddAuthorInputModel> _validator = validator;


    public async Task<OneOf<bool, Errors>> CreateAuthor(AddAuthorInputModel author)
    {
         var result = await _validator.ValidateAsync(author);

        if (!result.IsValid)
        {
            return new Errors(
                result.Errors.Select
                (
                    a => new AppErro(a.ErrorMessage, a.PropertyName)
            ).ToList());
        }
           

        return await _authorRepository.Create(author.Adapt<Author>());
    }

    public async Task<OneOf<bool, Errors>> DeleteById(string id)
    {
        return await _authorRepository.DeleteById(id);
    }

    public async Task<List<AuthorViewModel>> GetAuthor()
    {
        List<Author> authors = await this._authorRepository.GetAllAsync();


        return authors.Adapt<List<AuthorViewModel>>();
    }

    public async Task<OneOf<AuthorViewModel, Errors>> GetById(string id)
    {


        Author result = await this._authorRepository.GetById(id);

        return result.Adapt<AuthorViewModel>();
    }

    public async Task<OneOf<AuthorViewModel, Errors>> Update(AddAuthorInputModel author, string id)
    {
        
       Author result = await this._authorRepository.Update(author.Adapt<Author>(),id);

        return result.Adapt<AuthorViewModel>();

    }
}
