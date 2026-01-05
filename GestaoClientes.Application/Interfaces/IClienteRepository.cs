using GestaoClientes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Application.Interfaces
{
    public interface IClienteRepository
    {
        Task<bool> ExisteCnpjAsync(string cnpj);
        Task AdicionarAsync(Cliente cliente);
        Task<Cliente?> ObterPorIdAsync(Guid id);
    }
}
