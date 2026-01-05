using GestaoClientes.Domain.Exceptions;
using GestaoClientes.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Domain.Entities
{
    public class Cliente
    {
        protected Cliente() { } 

        public virtual Guid Id { get; protected set; }

        public virtual string Nome { get; protected set; } = null!;

        public virtual Cnpj Cnpj { get; protected set; } = null!;

        public virtual bool Ativo { get; protected set; }

        public Cliente(string nome, Cnpj cnpj)
        {
            AlterarNome(nome);
            Cnpj = cnpj;
            Id = Guid.NewGuid();
            Ativo = true;
        }

        public virtual void AlterarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome é obrigatório.");

            Nome = nome;
        }

        public virtual void Ativar() => Ativo = true;

        public virtual void Desativar() => Ativo = false;
    }

}
