using Beblue.Desafio.Cashback.Generico.Configuration;
using Beblue.Desafio.Cashback.Generico.Helpers;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Configuration;

namespace Beblue.Desafio.Cashback.Generico.Connection
{
    public class NHibernateFactory : IDisposable
    {
        private readonly ISessionFactory _sessionFactory;
        private ISession Session { get; set; }

        protected NHibernateFactory(Config config, ISession session)
        {
            Session = session;

            if (config.Connection.RunMigrations)
                MigrationHelper.Run(config.Connection.Business);

            try
            {
                var dll = AssemblyHelper.GetAssemblyByName(config.Connection.Business);

                var persistenceConfigurer = GetDatabase(config.Connection.DatabaseType);

                var fluentyConfigure = Fluently.Configure()
                    .Database(persistenceConfigurer)
                    .Mappings(m => m.FluentMappings.AddFromAssembly(dll));

                _sessionFactory = fluentyConfigure
                    .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(false, false, false))
                    .BuildConfiguration()
                    .BuildSessionFactory();

                Console.WriteLine("Conexão estabelicida com sucesso!");
            }
            catch (HibernateException exception)
            {
                throw new Exception("Erro ao estabelecer conexão com o banco de dados :", exception);
            }
        }

        private static IPersistenceConfigurer GetDatabase(string databaseType)
        {
            return databaseType switch
            {
                "sqlite" => SQLiteConfiguration.Standard.InMemory().ShowSql().FormatSql(),
                _ => throw new ConfigurationErrorsException($"{databaseType} platform is not supported by nhibernate facility.")
            };
        }

        public void Dispose()
        {
            Session?.Dispose();
        }

        public ISession OpenSession()
        {
            return _sessionFactory.OpenSession();
        }
    }
}
