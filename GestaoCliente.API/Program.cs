using GestaoClientes.API.Middleware;
using GestaoClientes.Application.Clientes.Commands;
using GestaoClientes.Application.Clientes.Queries;
using GestaoClientes.Application.Interfaces;
using GestaoClientes.Infrastructure.NHibernate;
using GestaoClientes.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();


var sessionFactory = NHibernateSessionFactory.Criar();
builder.Services.AddSingleton<ISessionFactory>(sessionFactory);

builder.Services.AddScoped<IClienteRepository, ClienteRepositoryNH>();
builder.Services.AddScoped<CriarClienteCommandHandler>();
builder.Services.AddScoped<ObterClientePorIdQueryHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestão de Clientes API V1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseHttpsRedirection(); 
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();
