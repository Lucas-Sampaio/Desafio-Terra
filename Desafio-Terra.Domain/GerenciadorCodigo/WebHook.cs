namespace Desafio_Terra.Domain.GerenciadorCodigo;

public class WebHook
{

    public string Type { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public bool? Active { get; set; }
    public List<string> Events { get; set; }
    public Config Config { get; set; }
    public DateTime Updated_at { get; set; }
    public DateTime Created_at { get; set; }
    public string Url { get; set; }
    public string TestUrl { get; set; }
    public string PingUrl { get; set; }
    public string DeliveriesUrl { get; set; }
    public LastResponse LastResponse { get; set; }
}

public class Config
{
    public string Content_type { get; set; }
    public string Insecure_ssl { get; set; }
    public string Url { get; set; }
}

public class LastResponse
{
    public object Code { get; set; }
    public string Status { get; set; }
    public object Message { get; set; }
}