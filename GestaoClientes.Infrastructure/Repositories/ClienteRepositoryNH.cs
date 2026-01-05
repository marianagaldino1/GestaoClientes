using GestaoClientes.Application.Interfaces;
using GestaoClientes.Domain.Entities;
using GestaoClientes.Infrastructure.NHibernate;
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

        public ClienteRepositoryNH(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task AdicionarAsync(Cliente cliente)
        {
            using var session = _sessionFactory.AbrirSessao();
            using var tx = session.BeginTransaction();

            await session.SaveAsync(cliente);
            await tx.CommitAsync();
        }

        public async Task<Cliente?> ObterPorIdAsync(Guid id)
        {
            using var session = _sessionFactory.AbrirSessao();
            return await session.GetAsync<Cliente>(id);
        }

        public async Task<bool> ExisteCnpjAsync(string cnpj)
        {
            using var session = _sessionFactory.AbrirSessao();

            return await session.Query<Cliente>()
                                .AnyAsync(c => c.Cnpj.Valor == cnpj);
        }
    }
}