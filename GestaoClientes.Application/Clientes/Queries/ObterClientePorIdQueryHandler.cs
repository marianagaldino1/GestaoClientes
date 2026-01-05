using GestaoClientes.Application.DTOs;
using GestaoClientes.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Application.Clientes.Queries
{
    public class ObterClientePorIdQueryHandler
    {
        private readonly IClienteRepository _repository;
        private readonly ILogger<ObterClientePorIdQueryHandler> _logger;

        public ObterClientePorIdQueryHandler(IClienteRepository repository, ILogger<ObterClientePorIdQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ClienteDto> HandleAsync(ObterClientePorIdQuery query)
        {
            try
            {
                var cliente = await _repository.ObterPorIdAsync(query.Id);

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente não encontrado. Id: {Id}", query.Id);
                    return null;
                }

                _logger.LogInformation("Cliente consultado com sucesso. Id: {Id}, Nome: {Nome}", cliente.Id, cliente.Nome);

                return new ClienteDto
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Cnpj = cliente.Cnpj.Valor,
                    Ativo = cliente.Ativo
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar cliente. Id: {Id}", query.Id);
                throw;
            }
        }

    }
}
