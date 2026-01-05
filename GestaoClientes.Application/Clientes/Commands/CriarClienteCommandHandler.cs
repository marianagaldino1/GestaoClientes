using GestaoClientes.Application.Interfaces;
using GestaoClientes.Domain.Entities;
using GestaoClientes.Domain.Exceptions;
using GestaoClientes.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Application.Clientes.Commands
{
    public class CriarClienteCommandHandler
    {
        private readonly IClienteRepository _repository;
        private readonly ILogger<CriarClienteCommandHandler> _logger;

        public CriarClienteCommandHandler(IClienteRepository repository, ILogger<CriarClienteCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Guid> HandleAsync(CriarClienteCommand command)
        {
            try
            {
                var cnpj = Cnpj.Criar(command.Cnpj);

                if (await _repository.ExisteCnpjAsync(cnpj.Valor))
                {
                    _logger.LogWarning("Tentativa de criar cliente com CNPJ duplicado: {Cnpj}", cnpj.Valor);
                    throw new DomainException("Já existe cliente com este CNPJ.");
                }

                var cliente = new Cliente(command.Nome, cnpj);
                await _repository.AdicionarAsync(cliente);

                _logger.LogInformation("Cliente criado com sucesso. Id: {Id}, Nome: {Nome}", cliente.Id, cliente.Nome);

                return cliente.Id;
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Erro de domínio ao criar cliente: {@Command}", command);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao criar cliente: {@Command}", command);
                throw;
            }
        }
    }
}
