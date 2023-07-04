namespace Desafio_Terra.API.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Chama o próximo middleware na cadeia
            await _next(context);
        }
        catch (Exception ex)
        {
            // Lida com o erro e retorna uma resposta adequada
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Aqui você pode implementar a lógica para lidar com o erro
        // Pode retornar uma resposta com um status code específico, por exemplo.

        // Exemplo: retornando um status code 500 (Internal Server Error)
        context.Response.StatusCode = 500;
        return context.Response.WriteAsync("Ocorreu um erro interno. Por favor, tente novamente mais tarde.");
    }
}
