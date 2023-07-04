using Desafio_Terra.API.Models;
using Desafio_Terra.Domain.GerenciadorCodigo;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Desafio_Terra.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RepositorioController : ControllerBase
{
    private IGerenciadorCodigoService _gerenciadorCodigoService;

    public RepositorioController(IGerenciadorCodigoService gerenciadorCodigoService)
    {
        _gerenciadorCodigoService = gerenciadorCodigoService;
    }

    // POST api/<RepositorioController>
    /// <summary>
    /// Adicionar um novo repositorio a um usario do token
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PostAdicionarRepo([FromBody] CriarRepoRequest model)
    {
        var (url, erro) = await _gerenciadorCodigoService.CriarRepositorio(model.Nome, model.Descricao);

        if (!string.IsNullOrWhiteSpace(erro))
            return BadRequest(erro);

        return Created(url, null);
    }

    // GET api/<RepositorioController>/user25/repoteste/branches
    /// <summary>
    /// obtem as branchs de um repositorio
    /// </summary>
    /// <param name="owner">dono do repositorio</param>
    /// <param name="repo">nome do repositorio</param>
    /// <returns></returns>
    [HttpGet("{owner}/{repo}/branches")]
    public async Task<IActionResult> GetBranchs(string owner, string repo)
    {
        var (branchs, erro) = await _gerenciadorCodigoService.ObterBranchs(owner, repo);

        if (!string.IsNullOrWhiteSpace(erro))
            return BadRequest(erro);

        return Ok(branchs);
    }

    // GET api/<RepositorioController>/user25/repoteste/branches
    /// <summary>
    /// obtem os webhooks de um repositorio
    /// </summary>
    /// <param name="owner">dono do repositorio</param>
    /// <param name="repo">nome do repositorio</param>
    /// <returns></returns>
    [HttpGet("{owner}/{repo}/hooks")]
    public async Task<IActionResult> GetWebHooks(string owner, string repo)
    {
        var (hooks, erro) = await _gerenciadorCodigoService.ObterWebhooks(owner, repo);

        if (!string.IsNullOrWhiteSpace(erro))
            return BadRequest(erro);

        return Ok(hooks);
    }

    /// <summary>
    /// Cria um novo webhook para o repositorio
    /// </summary>
    /// <param name="owner">dono do repositorio</param>
    /// <param name="repo">nome do repositorio</param>
    /// <param name="criarWebhook">json para criar o webhook</param>
    /// <returns></returns>
    [HttpPost("{owner}/{repo}/hooks")]
    public async Task<IActionResult> PostWebHooks(string owner, string repo, [FromBody] CriarWebhookRequest criarWebhook)
    {
        var webHook = new WebHook
        {
            Active = criarWebhook.AtivarNotificacoes,
            Events = criarWebhook.Eventos,
            Config = new Config
            {
                Content_type = criarWebhook.Config?.Tipo,
                Url = criarWebhook.Config?.Url,
            }
        };

        var (hooks, erro) = await _gerenciadorCodigoService.AdicionarWebhook(owner, repo, webHook);

        if (!string.IsNullOrWhiteSpace(erro))
            return BadRequest(erro);

        return Created(hooks.Url, hooks);
    }

    /// <summary>
    /// Atualiza webhook de um repositorio
    /// </summary>
    /// <param name="owner">dono do repositorio</param>
    /// <param name="repo">nome do repositorio</param>
    /// <param name="id">id do webhook</param>
    /// <param name="attWebhook">json informe as propriedades que serao atualizadas</param>
    /// <returns></returns>
    [HttpPatch("{owner}/{repo}/hooks/{id}")]
    public async Task<IActionResult> PatchWebHooks(string owner, string repo, int id, [FromBody] AtualizarWebhookRequest attWebhook)
    {
        var webHook = new WebHook
        {
            Active = attWebhook.AtivarNotificacoes,
            Events = attWebhook.Eventos
        };

        if (attWebhook.Config is not null)
        {
            webHook.Config = new Config
            {
                Content_type = attWebhook.Config.Tipo,
                Url = attWebhook.Config.Url
            };
        }

        var (hooks, erro) = await _gerenciadorCodigoService.
            AtualizarWebhook(owner, repo, id, webHook, attWebhook.AddEventos, attWebhook.RemoveEventos);

        if (!string.IsNullOrWhiteSpace(erro))
            return BadRequest(erro);

        return Ok(hooks);
    }
}