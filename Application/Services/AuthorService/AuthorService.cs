using Application.Dtos.FollowDTO;
using Application.Dtos.Models;
using Application.IRepository.IAuthorRepository;
using Application.IRepository.IFollowRepository;
using Application.IServices;
using Domain.Entities;
using Domain.Erros;
using FluentValidation;
using Mapster;
using OneOf;

namespace Application.Services.AuthorService;

public class AuthorService(
    IAuthorRepository authorRepository,
    IValidator<AuthorCreateDTO> validator,
    IUnitOfWork.IUnitOfWork unitOfWor,
    IFollowRepository followRepository
    ) : IServiceAuthor
{
    private readonly IAuthorRepository _authorRepository = authorRepository;
    private readonly IValidator<AuthorCreateDTO> _validator = validator;
    private readonly IUnitOfWork.IUnitOfWork _unitOfWork = unitOfWor;
    private readonly IFollowRepository _followRepository = followRepository;



    public async Task<OneOf<AuthorReadDTO, Errors>> CreateAuthorAsync(AuthorCreateDTO dtoCreate)
    {

        if (Errors.TryValid(await _validator.ValidateAsync(dtoCreate), out Errors errors))
            return errors;

        var author =  Author.CreateAuthor(dtoCreate.Name, BCrypt.Net.BCrypt.HashPassword(dtoCreate.Password), dtoCreate.Email);

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

    public async Task<OneOf<FollowReadDTO, Errors>> CreateFollowAsync(FollowCreateDTO follow)
    {
        if (follow.FollowerId.Equals(follow.FollowingId))
            return Errors.EmiteError("não é possivel seguir você mesmo", nameof(Follow));

        Author? follower = await this._authorRepository.GetAuthorByIdAsync(follow.FollowerId);
        Author? following = await this._authorRepository.GetAuthorByIdAsync(follow.FollowingId);



        if (follower is null || following is null)
           return Errors.EmiteError("follower e following não pode ser nulos", nameof(Follow));

      


       Follow foll = Follow.CreateFollow(follow.FollowerId, follow.FollowingId);


        await _followRepository.CreateFollow(foll);

        await this._unitOfWork.SaveAsync();

        return foll.Map();
    }
}
