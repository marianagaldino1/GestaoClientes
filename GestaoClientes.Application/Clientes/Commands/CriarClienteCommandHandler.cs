using GestaoClientes.Application.Interfaces;
using GestaoClientes.Domain.Entities;
using GestaoClientes.Domain.Exceptions;
using GestaoClientes.Domain.ValueObjects;
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

        public CriarClienteCommandHandler(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> HandleAsync(CriarClienteCommand command)
        {
            var cnpj = Cnpj.Criar(command.Cnpj);

            if (await _repository.ExisteCnpjAsync(cnpj.Valor))
                throw new DomainException("Já existe cliente com este CNPJ.");

            var cliente = new Cliente(command.Nome, cnpj);
            await _repository.AdicionarAsync(cliente);


            return cliente.Id;
        }

    }
}
