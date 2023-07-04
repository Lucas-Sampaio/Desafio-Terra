using System.ComponentModel.DataAnnotations;

namespace Desafio_Terra.API.Models;

public class CriarRepoRequest
{
    [Required]
    [MinLength(1,ErrorMessage ="O campo {0} tem que ter um carctere minimo ")]
    /// <summary>
    /// Nome do repositorio.
    /// </summary>
    public string Nome { get; set; }
    /// <summary>
    /// Descricao do repositorio opcional.
    /// </summary>
    public string? Descricao { get; set; }
}
