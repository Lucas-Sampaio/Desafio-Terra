using Desafio_Terra.API.Controllers;
using Desafio_Terra.API.Models;
using Desafio_Terra.Domain.GerenciadorCodigo;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Desafio_Terra.Test;

public class RepositorioControllerTest
{
    private Mock<IGerenciadorCodigoService> _gerenciadorCodigoServiceMock;
    private RepositorioController _repoController;

    public RepositorioControllerTest()
    {
        _gerenciadorCodigoServiceMock = new Mock<IGerenciadorCodigoService>();
        _repoController = new RepositorioController(_gerenciadorCodigoServiceMock.Object);
    }

    [Fact]
    public async Task AdicionarRepo_Ocorre_com_sucesso()
    {
        //arrange
        var request = new CriarRepoRequest
        {
            Nome = "Teste repo",
            Descricao = "Descricao teste"
        };
        var urlResponse = "https://testeurl.com";

        _ = _gerenciadorCodigoServiceMock.Setup
            (
            x => x.CriarRepositorio(
                It.Is<string>(y => y == request.Nome),
                It.Is<string>(y => y == request.Descricao))
            ).ReturnsAsync((urlResponse, ""));

        //action
        var response = await _repoController.PostAdicionarRepo(request);

        //action
        Assert.IsType<CreatedResult>(response);
        Assert.Equal(urlResponse, ((CreatedResult)response).Location);

        _gerenciadorCodigoServiceMock.Verify
          (
         x => x.CriarRepositorio(
                It.Is<string>(y => y == request.Nome),
                It.Is<string>(y => y == request.Descricao))
          , Times.Once);
    }

    [Fact]
    public async Task AdicionarRepo_Ocorre_com_erro()
    {
        //arrange
        var request = new CriarRepoRequest
        {
            Nome = "Teste repo",
            Descricao = "Descricao teste"
        };
        var erro = "Ocorreu um erro interno";
        _ = _gerenciadorCodigoServiceMock.Setup
            (
            x => x.CriarRepositorio(
                It.Is<string>(y => y == request.Nome),
                It.Is<string>(y => y == request.Descricao))
            ).ReturnsAsync(("", "Ocorreu um erro interno"));

        //action
        var response = await _repoController.PostAdicionarRepo(request);

        //action
        Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal(erro, ((BadRequestObjectResult)response).Value);

        _gerenciadorCodigoServiceMock.Verify
          (
         x => x.CriarRepositorio(
                It.Is<string>(y => y == request.Nome),
                It.Is<string>(y => y == request.Descricao))
          , Times.Once);
    }

    [Fact]
    public async Task ListarBranchsrRepo_Ocorre_com_sucesso()
    {
        //arrange
        var owner = "dono teste";
        var repo = "repo teste";

        var branchsResponse = new List<Branch>
        {
            new Branch { Nome = "Branch1"},
            new Branch { Nome = "Branch2"}
        };

        _ = _gerenciadorCodigoServiceMock.Setup
            (
            x => x.ObterBranchs(
                It.Is<string>(y => y == owner),
                It.Is<string>(y => y == repo))
            ).ReturnsAsync((branchsResponse, ""));

        //action
        var response = await _repoController.GetBranchs(owner, repo);

        //assert
        Assert.IsType<OkObjectResult>(response);
        Assert.NotNull(((OkObjectResult)response).Value);

        _gerenciadorCodigoServiceMock.Verify
            (
            x => x.ObterBranchs(
                It.Is<string>(y => y == owner),
                It.Is<string>(y => y == repo))
            , Times.Once);
    }

    [Fact]
    public async Task ListarWebhookRepo_Ocorre_com_sucesso()
    {
        //arrange
        var owner = "dono teste";
        var repo = "repo teste";

        var request = new CriarWebhookRequest();

        var webhooksResponse = new List<WebHook>()
        {
           new WebHook()
        };

        _ = _gerenciadorCodigoServiceMock.Setup
            (
            x => x.ObterWebhooks(
                It.Is<string>(y => y == owner),
                It.Is<string>(y => y == repo))
            ).ReturnsAsync((webhooksResponse, ""));

        //action
        var response = await _repoController.GetWebHooks(owner, repo);

        //action
        Assert.IsType<OkObjectResult>(response);
        Assert.NotNull(((OkObjectResult)response).Value);
        _gerenciadorCodigoServiceMock.Verify
          (
        x => x.ObterWebhooks(
                It.Is<string>(y => y == owner),
                It.Is<string>(y => y == repo))
          , Times.Once);
    }

    [Fact]
    public async Task AdicionarWebhook_Ocorre_com_sucesso()
    {
        //arrange
        var owner = "dono teste";
        var repo = "repo teste";

        var request = new CriarWebhookRequest();

        var webhookResponse = new WebHook()
        {
            Url = "https://testeurl.com"
        };

        _ = _gerenciadorCodigoServiceMock.Setup
            (
            x => x.AdicionarWebhook(
                It.Is<string>(y => y == owner),
                It.Is<string>(y => y == repo),
                It.IsAny<WebHook>())
            ).ReturnsAsync((webhookResponse, ""));

        //action
        var response = await _repoController.PostWebHooks(owner, repo, request);

        //action
        Assert.IsType<CreatedResult>(response);
        Assert.Equal(webhookResponse.Url, ((CreatedResult)response).Location);
        Assert.NotNull(((CreatedResult)response).Value);
        _gerenciadorCodigoServiceMock.Verify
          (
         x => x.AdicionarWebhook(
                It.Is<string>(y => y == owner),
                It.Is<string>(y => y == repo),
                It.IsAny<WebHook>())
          , Times.Once);
    }

    [Fact]
    public async Task AtualizarWebhookRepo_Ocorre_com_sucesso()
    {
        //arrange
        var owner = "dono teste";
        var repo = "repo teste";
        var webhookId = 1;
        var request = new AtualizarWebhookRequest();

        var webhookResponse = new WebHook()
        {
            Url = "https://testeurl.com"
        };

        _ = _gerenciadorCodigoServiceMock.Setup
            (
            x => x.AtualizarWebhook(
                It.Is<string>(y => y == owner),
                It.Is<string>(y => y == repo),
                It.Is<int>(y => y == webhookId),
                It.IsAny<WebHook>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>())
            ).ReturnsAsync((webhookResponse, ""));

        //action
        var response = await _repoController.PatchWebHooks(owner, repo, webhookId, request);

        //action
        Assert.IsType<OkObjectResult>(response);
        Assert.NotNull(((OkObjectResult)response).Value);
        _gerenciadorCodigoServiceMock.Verify
          (
         x => x.AtualizarWebhook(
                It.Is<string>(y => y == owner),
                It.Is<string>(y => y == repo),
                It.Is<int>(y => y == webhookId),
                It.IsAny<WebHook>(),
                It.IsAny<List<string>>(),
                It.IsAny<List<string>>())
          , Times.Once);
    }
}