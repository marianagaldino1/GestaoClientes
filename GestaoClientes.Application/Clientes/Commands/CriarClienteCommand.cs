using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Application.Clientes.Commands
{
    public record CriarClienteCommand
    {
        public string Nome { get; init; } = null!;
        public string Cnpj { get; init; } = null!;

        public CriarClienteCommand() { }

        public CriarClienteCommand(string nome, string cnpj)
        {
            Nome = nome;
            Cnpj = cnpj;
        }
    }

}
