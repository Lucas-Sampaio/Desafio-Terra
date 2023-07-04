namespace Desafio_Terra.API.Models;

public class CriarWebhookRequest
{
    /// <summary>
    /// Determina para quais eventos o Webhook é acionado.
    /// </summary>
    public List<string>? Eventos { get; set; }
    /// <summary>
    /// Determina se as notificações são enviadas quando o webhook é acionado.
    /// </summary>
    public bool AtivarNotificacoes { get; set; }
    public ConfigWebHookModel? Config { get; set; }
}
