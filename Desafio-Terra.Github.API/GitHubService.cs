using Desafio_Terra.Core;
using Desafio_Terra.Domain.GerenciadorCodigo;
using Desafio_Terra.Github.API.Responses;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Desafio_Terra.Github.API;

public class GitHubService : ServiceBase, IGerenciadorCodigoService
{
    private readonly HttpClient _httpClient;

    public GitHubService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        _httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("TerraTest.NetApi");
    }

    public async ValueTask<(WebHook? webHook, string? erro)> AdicionarWebhook(string donoRepo, string nomeRepo, WebHook webHook)
    {
        var endPoint = $"repos/{donoRepo}/{nomeRepo}/hooks";

        var newWebhook = new
        {
            name = "web",
            active = webHook.Active,
            events = webHook.Events,
            config = new
            {
                url = webHook.Config.Url,
                content_type = webHook.Config.Content_type
            }
        };

        var response = await _httpClient.PostAsync(endPoint, ObterConteudo(newWebhook));

        if (!response.IsSuccessStatusCode)
            return (null, await ObterErro(response));

        var conteudo = await DeserializarObjetoResponse<WebHookResponse>(response);
        return (Parse(conteudo), null);
    }

    public async ValueTask<(WebHook? webHook, string? erro)> AtualizarWebhook(string donoRepo, string nomeRepo, int id, WebHook webHook,
        List<string>? addEventos = null, List<string>? removeEventos = null)
    {
        var endPoint = $"repos/{donoRepo}/{nomeRepo}/hooks/{id}";

        var newWebhook = new
        {
            active = webHook.Active,
            events = webHook.Events,
            config = webHook.Config != null ? new
            {
                url = webHook.Config.Url,
                content_type = webHook.Config.Content_type
            } : null,
            add_events = addEventos,
            remove_events = removeEventos
        };

        var options = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var response = await _httpClient.PatchAsync(endPoint, ObterConteudo(newWebhook, options));

        if (!response.IsSuccessStatusCode)
            return (null, await ObterErro(response));

        var conteudo = await DeserializarObjetoResponse<WebHookResponse>(response);
        return (Parse(conteudo), null);
    }

    public async ValueTask<(string? repoUrl, string? erro)> CriarRepositorio(string nomeRepo, string descricao = "")
    {
        var endPoint = $"user/repos";
        // Dados do novo repositório
        var newRepoData = new
        {
            name = nomeRepo,
            description = descricao,
        };

        var response = await _httpClient.PostAsync(endPoint, ObterConteudo(newRepoData));

        if (!response.IsSuccessStatusCode)
            return (null, await ObterErro(response));

        var conteudo = await DeserializarObjetoResponse<RepositorioCriadoResponse>(response);
        return (conteudo?.html_url, null);
    }

    public async ValueTask<(List<Branch>? branchs, string? erro)> ObterBranchs(string donoRepo, string nomeRepo)
    {
        var endPoint = $"repos/{donoRepo}/{nomeRepo}/branches";
        var response = await _httpClient.GetAsync(endPoint);

        if (!response.IsSuccessStatusCode)
            return (null, await ObterErro(response));

        var conteudo = await DeserializarObjetoResponse<List<BranchResponse>>(response);
        var branchs = conteudo.Select(x => new Branch { Nome = x.name });

        return (branchs.ToList(), null);
    }

    public async ValueTask<(List<WebHook>? webHooks, string? erro)> ObterWebhooks(string donoRepo, string nomeRepo)
    {
        var endPoint = $"repos/{donoRepo}/{nomeRepo}/hooks";
        var response = await _httpClient.GetAsync(endPoint);

        if (!response.IsSuccessStatusCode)
            return (null, await ObterErro(response));

        var conteudo = await DeserializarObjetoResponse<List<WebHookResponse>>(response);
        var hooks = conteudo.Select(Parse);

        return (hooks.ToList(), null);
    }

    private async ValueTask<string> ObterErro(HttpResponseMessage message)
    {
        var erro = await message.Content.ReadAsStringAsync();
        return erro;
    }

    private WebHook Parse(WebHookResponse hookResponse)
    {
        var hook = new WebHook()
        {
            Active = hookResponse.active,
            Created_at = hookResponse.created_at,
            DeliveriesUrl = hookResponse.deliveries_url,
            Events = hookResponse.events,
            Id = hookResponse.id,
            Name = hookResponse.name,
            PingUrl = hookResponse.ping_url,
            TestUrl = hookResponse.test_url,
            @Type = hookResponse.type,
            Updated_at = hookResponse.updated_at,
            Url = hookResponse.url,
            Config = new Domain.GerenciadorCodigo.Config
            {
                Url = hookResponse.config.url,
                Content_type = hookResponse.config.content_type,
                Insecure_ssl = hookResponse.config.insecure_ssl
            },
            LastResponse = new Domain.GerenciadorCodigo.LastResponse
            {
                Code = hookResponse.last_response.code,
                Message = hookResponse.last_response.message,
                Status = hookResponse.last_response.status,
            }
        };

        return hook;
    }
}