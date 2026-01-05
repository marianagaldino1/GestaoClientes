using GestaoClientes.Infrastructure.NHibernate.Mappings;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoClientes.Infrastructure.NHibernate
{
    public static class NHibernateSessionFactory
    {
        private static ISessionFactory? _sessionFactory;
        private static SQLiteConnection? _connection;

        public static ISessionFactory Criar()
        {
            if (_sessionFactory != null)
                return _sessionFactory;

            _connection = new SQLiteConnection("Data Source=:memory:;Version=3;New=True;");
            _connection.Open();

            var cfg = new Configuration();
            cfg.DataBaseIntegration(db =>
            {
                db.Driver<SQLite20Driver>();
                db.Dialect<SQLiteDialect>();
                db.ConnectionString = _connection.ConnectionString;
                db.LogSqlInConsole = true;
            });

            var mapper = new ModelMapper();
            mapper.AddMapping<ClienteMap>();
            cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            new SchemaExport(cfg).Execute(false, true, false, _connection, null);

            _sessionFactory = cfg.BuildSessionFactory();
            return _sessionFactory;
        }

        public static ISession AbrirSessao(this ISessionFactory factory)
        {
            if (_sessionFactory == null) Criar();
            return _sessionFactory!.OpenSession(_connection);
        }
    }
}
