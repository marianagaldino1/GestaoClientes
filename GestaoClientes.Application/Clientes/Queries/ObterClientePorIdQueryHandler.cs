using GestaoClientes.Application.DTOs;
using GestaoClientes.Application.Interfaces;
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

        public ObterClientePorIdQueryHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClienteDto?> HandleAsync(ObterClientePorIdQuery query)
        {
            var cliente = await _repository.ObterPorIdAsync(query.Id);

            if (cliente == null)
                return null;

            return new ClienteDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Cnpj = cliente.Cnpj.Valor,
                Ativo = cliente.Ativo
            };
        }
    }
}
