namespace Desafio_Terra.API.Models;

public class AtualizarWebhookRequest
{
    /// <summary>
    /// Determina para quais eventos o Webhook é acionado.
    /// Isso substitui toda a matriz de eventos.
    /// </summary>
    public List<string>? Eventos { get; set; }
    /// <summary>
    /// Determina uma lista de  eventos a ser adicionado no Webhook.
    /// </summary>
    public List<string>? AddEventos { get; set; }
    /// <summary>
    /// Determina uma lista de  eventos a ser removidos do Webhook.
    /// </summary>
    public List<string>? RemoveEventos { get; set; }
    /// <summary>
    /// Determina se as notificações são enviadas quando o webhook é acionado.
    /// </summary>
    public bool? AtivarNotificacoes { get; set; }
    public ConfigWebHookModel? Config { get; set; }

}