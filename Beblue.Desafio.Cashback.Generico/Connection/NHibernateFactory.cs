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
    public class NHibernateFactory
    {
        private ISessionFactory SessionFactory { get; set; }

        public NHibernateFactory(ConnectionConfig connectionConfig)
        {
            if (connectionConfig.RunMigrations)
                MigrationHelper.Run(connectionConfig.Business, connectionConfig.ConnectionString);

            try
            {
                var dll = AssemblyHelper.GetAssemblyByName(connectionConfig.Business);
                var persistenceConfigurer = GetDatabase(connectionConfig.DatabaseType, connectionConfig.ConnectionString);

                var fluentyConfigure = Fluently.Configure()
                    .Database(persistenceConfigurer)
                    .Mappings(m => m.FluentMappings.AddFromAssembly(dll));

                SessionFactory = fluentyConfigure
                    .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(false, false, false))
                    .BuildConfiguration()
                    .BuildSessionFactory();

                Console.WriteLine("Connection established successfully");
            }
            catch (HibernateException exception)
            {
                throw new Exception("Error when trying to establish database connection :", exception);
            }
        }

        private IPersistenceConfigurer GetDatabase(string databaseType, string connectionString)
        {
            return databaseType switch
            {
                "sqlite" => SQLiteConfiguration.Standard.ConnectionString(connectionString).ShowSql().FormatSql(),
                _ => throw new ConfigurationErrorsException($"{databaseType} platform not supported by system.")
            };
        }

        public ISession OpenSession() => SessionFactory.OpenSession();
    }
}
