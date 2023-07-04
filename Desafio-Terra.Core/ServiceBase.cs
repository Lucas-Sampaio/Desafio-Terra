using System.Text;
using System.Text.Json;

namespace Desafio_Terra.Core;

public class ServiceBase
{
    protected StringContent ObterConteudo(object dado, JsonSerializerOptions? options = null)
    {
        return new StringContent(JsonSerializer.Serialize(dado, options: options), Encoding.UTF8, "application/vnd.github+json") ;
    }

    protected async ValueTask<T> DeserializarObjetoResponse<T>(HttpResponseMessage response)
    {
        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), options);
    }
}