namespace Desafio_Terra.API.Models;

public class ConfigWebHookModel
{
    /// <summary>
    /// A URL para a qual os payloads serão entregues.
    /// </summary>
    public string Url { get; set; }
    /// <summary>
    /// Tipo do  payload. Ex: json,form.
    /// </summary>
    public string Tipo { get; set; }
}
