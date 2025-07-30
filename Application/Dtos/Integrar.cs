
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos;

public record  Integrar( Guid id, List<FileDocs> files, TypeFile type);
