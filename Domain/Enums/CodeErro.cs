using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;

/// <summary>
/// Status de erros utilizados pela aplicação.
/// Valores numéricos selecionados para facilitar mapeamento com códigos HTTP/serviço quando aplicável.
/// </summary>
public enum CodeErro
{
    /// <summary>
    /// Sem erro / estado padrão.
    /// </summary>
    Nenhum = 0,

    /// <summary>
    /// Erro desconhecido ou não categorizado.
    /// </summary>
    Desconhecido = 1,

    /// <summary>
    /// Requisição mal formada / parâmetros inválidos.
    /// </summary>
    BadRequest = 400,

    /// <summary>
    /// Falha de validação de dados de entrada.
    /// </summary>
    ValidationError = 422,

    /// <summary>
    /// Não autenticado.
    /// </summary>
    Unauthorized = 401,

    /// <summary>
    /// Autorização negada.
    /// </summary>
    Forbidden = 403,

    /// <summary>
    /// Recurso não encontrado.
    /// </summary>
    NotFound = 404,

    /// <summary>
    /// Conflito de estado (ex.: recurso já existente).
    /// </summary>
    Conflict = 409,

    /// <summary>
    /// Tempo de operação excedido.
    /// </summary>
    Timeout = 408,

    /// <summary>
    /// Dependência externa falhou.
    /// </summary>
    DependencyFailure = 424,

    /// <summary>
    /// Erro interno do servidor.
    /// </summary>
    InternalServerError = 500,

    /// <summary>
    /// Serviço indisponível.
    /// </summary>
    ServiceUnavailable = 503
}
