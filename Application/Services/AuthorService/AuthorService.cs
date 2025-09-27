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

    public async Task<OneOf<AuthorViewModel, Errors>> CreateAuthor(AddAuthorInputModel author)
    {
        var result = await _validator.ValidateAsync(author);

        if (!result.IsValid)
            return Errors.Factory.CreateErro(result.Errors.Select(a => new AppErro(a.ErrorMessage, a.PropertyName)));



        var authorData = await this._authorRepository.Create((Author)author);
        return authorData.Map();

    }

    public async Task<OneOf<bool, Errors>> DeleteById(string id)
    {
        var result = await _authorRepository.DeleteById(id);

        if (!result )
            return Errors.Factory.CreateErro([new AppErro("author não encontrado", nameof(Author))]);


        return result;
    }

    public async Task<List<AuthorViewModel>> GetAuthor()
    {
        List<Author> authors = await this._authorRepository.GetAllAsync();


        return authors.Map();
    }

    public async Task<OneOf<AuthorViewModel, Errors>> GetById(string id)
    {
        Author result = await this._authorRepository.GetById(id);

        if (result is null)
            return Errors.Factory.CreateErro([new AppErro("author não encontrado", nameof(Author))]);

        return result.Map();
    }

    public async Task<OneOf<AuthorViewModel, Errors>> Update(AddAuthorInputModel author, string id)
    {

        Author _author = await this._authorRepository.GetById(id);
        if (_author is null)
            return Errors.Factory.CreateErro([new AppErro("author não encontrado", nameof(Author))]);

        _author.UpdateName(author.Name);
        Author result = await this._authorRepository.Update(_author, id);

        return result.Map();

    }
}
