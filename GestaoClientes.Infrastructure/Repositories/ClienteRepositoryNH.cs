using GestaoClientes.Application.Interfaces;
using GestaoClientes.Domain.Entities;
using GestaoClientes.Infrastructure.NHibernate;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Infrastructure.Repositories
{
    public class ClienteRepositoryNH : IClienteRepository
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ILogger<ClienteRepositoryNH> _logger;

        public ClienteRepositoryNH(ISessionFactory sessionFactory, ILogger<ClienteRepositoryNH> logger)
        {
            _sessionFactory = sessionFactory;
            _logger = logger;
        }

        public async Task AdicionarAsync(Cliente cliente)
        {
            try
            {
                using var session = _sessionFactory.AbrirSessao();
                using var tx = session.BeginTransaction();

                await session.SaveAsync(cliente);
                await tx.CommitAsync();

                _logger.LogInformation("Cliente salvo com sucesso. Id: {Id}", cliente.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar cliente. Id: {Id}", cliente.Id);
                throw;
            }
        }

        public async Task<Cliente?> ObterPorIdAsync(Guid id)
        {
            using var session = _sessionFactory.AbrirSessao();
            var cliente = await session.GetAsync<Cliente>(id);

            if (cliente == null)
                _logger.LogWarning("Cliente {Id} não encontrado.", id);

            return cliente;
        }

        public async Task<bool> ExisteCnpjAsync(string cnpj)
        {
            using var session = _sessionFactory.AbrirSessao();

            var existe = await session.Query<Cliente>()
                                     .AnyAsync(c => c.Cnpj.Valor == cnpj);

            _logger.LogInformation("Busca por CNPJ {Cnpj}: {Existe}", cnpj, existe);
            return existe;
        }
    }
}