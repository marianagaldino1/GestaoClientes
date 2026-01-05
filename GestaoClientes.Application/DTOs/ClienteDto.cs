using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Application.DTOs
{
    public class ClienteDto
    {
        public Guid Id { get; init; }
        public string Nome { get; init; } = string.Empty;
        public string Cnpj { get; init; } = string.Empty;
        public bool Ativo { get; init; }
    }
}
