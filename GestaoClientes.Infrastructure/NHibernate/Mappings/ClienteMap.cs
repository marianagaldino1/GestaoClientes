using GestaoClientes.Domain.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Infrastructure.NHibernate.Mappings
{
    public class ClienteMap : ClassMapping<Cliente>
    {
        public ClienteMap()
        {
            Table("Clientes");

            Id(x => x.Id, m =>
            {
                m.Generator(Generators.GuidComb);
            });

            Property(x => x.Nome, m =>
            {
                m.NotNullable(true);
                m.Length(200);
            });

            Component(x => x.Cnpj, c =>
            {
                c.Property(p => p.Valor, m =>
                {
                    m.Column("Cnpj");
                    m.Length(14);
                    m.NotNullable(true);
                });
            });

            Property(x => x.Ativo);
        }
    }

}
