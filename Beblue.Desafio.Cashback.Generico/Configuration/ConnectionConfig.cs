namespace Beblue.Desafio.Cashback.Generico.Configuration
{
    public class ConnectionConfig
    {
        public string ConnectionString { get; set; }

        public string DatabaseType { get; set; }

        public string Business { get; set; }

        public bool RunMigrations { get; set; }
    }
}
