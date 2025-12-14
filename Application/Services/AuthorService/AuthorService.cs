using Application.Dtos.Models;
using Application.IServices;
using Domain.Entities;
using Domain.Erros;
using Domain.Erros.AppErro;
using Domain.IRepository.IAuthorRepository;
using FluentValidation;
using Infrastructure.UnitOfWork;
using Mapster;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.AuthorService;

public class AuthorService(
    IAuthorRepository authorRepository,
    IValidator<AuthorCreateDTO> validator,
    IUnitOfWork unitOfWor
    ) : IServiceAuthor
{
    private readonly IAuthorRepository _authorRepository = authorRepository;
    private readonly IValidator<AuthorCreateDTO> _validator = validator;
   private readonly IUnitOfWork _unitOfWork = unitOfWor;


    public async Task<OneOf<AuthorReadDTO, Errors>> CreateAuthorAsync(AuthorCreateDTO authorDTO)
    {

        if (Errors.TryValid(await _validator.ValidateAsync(authorDTO), out Errors errors))
            return errors;

        var author = (Author)authorDTO;

        await this._authorRepository.CreateAuthorAsync(author);

        await this._unitOfWork.SaveAsync();

        return author.Map();

    }

    public async Task<OneOf<bool, Errors>> RemoveAuthorByIdAsync(string id)
    {
        var author = await _authorRepository.GetAuthorByIdAsync(id);

        if (author is null)
            return Errors.EmiteError("author not faound",nameof(Author));

         _authorRepository.RemoveAuthor(author);

        return await _unitOfWork.SaveAsync() ;
    }

    public async Task<IEnumerable<AuthorReadDTO>> GetAllAuthorAsync()
    {
        return (await this._authorRepository.GetAllAuthorAsync()).Map();


    }

    public async Task<OneOf<AuthorReadDTO, Errors>> GetAuthorByIdAsync(string id)
    {
        Author? result = await this._authorRepository.GetAuthorByIdAsync(id);

        if (result is null)
            return Errors.EmiteError("author not faound", nameof(Author));

        return result.Map();
    }

    public async Task<OneOf<AuthorReadDTO, Errors>> UpdateAuthorAsync(AuthorCreateDTO author, string id)
    {

        Author? _author = await this._authorRepository.GetAuthorByIdAsync(id);
        if (_author is null)
            return Errors.EmiteError("author not faound", nameof(Author));

        _author.UpdateName(author.Name);
        await this._unitOfWork.SaveAsync();

        return _author.Map();

    }
}
