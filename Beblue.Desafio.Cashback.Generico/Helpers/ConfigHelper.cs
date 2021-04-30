using Beblue.Desafio.Cashback.Generico.Configuration;
using Beblue.Desafio.Cashback.Generico.Connection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Beblue.Desafio.Cashback.Generico.Helpers
{
    public static class ConfigHelper
    {
        public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionConfig = new ConnectionConfig();
            configuration.GetSection(nameof(Connection)).Bind(connectionConfig);

            services.AddSingleton(connectionConfig);
            services.AddSingleton<NHibernateFactory>();
        }
    }
}
