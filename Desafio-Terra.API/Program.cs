using Desafio_Terra.API.Middleware;
using Desafio_Terra.Domain.GerenciadorCodigo;
using Desafio_Terra.Github.API;
using System.Net.Http.Headers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var token = builder.Configuration.GetValue<string>("Personal_token");
var baseUrl = builder.Configuration.GetValue<string>("Base_url_git");

builder.Services.AddHttpClient<IGerenciadorCodigoService, GitHubService>()
    .ConfigureHttpClient(x =>
    {
        x.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
        x.BaseAddress = new Uri(baseUrl);
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    // Configuração para processar os comentários do XML da documentação do projeto
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    setup.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.Run();