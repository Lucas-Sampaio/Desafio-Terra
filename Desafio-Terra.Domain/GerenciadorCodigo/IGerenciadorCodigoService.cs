namespace Desafio_Terra.Domain.GerenciadorCodigo;

public interface IGerenciadorCodigoService
{
    ValueTask<(string repoUrl, string erro)> CriarRepositorio(string nomeRepo, string descricao = "");
    ValueTask<(List<Branch> branchs, string erro)> ObterBranchs(string donoRepo, string nomeRepo);
    ValueTask<(List<WebHook> webHooks, string erro)> ObterWebhooks(string donoRepo, string nomeRepo);
    ValueTask<(WebHook webHook, string erro)> AdicionarWebhook(string donoRepo, string nomeRepo, WebHook webHook);
    ValueTask<(WebHook webHook, string erro)> AtualizarWebhook
        (string donoRepo, string nomeRepo, int id, WebHook webHook, List<string>? addEventos = null, List<string>? removeEventos = null);
}