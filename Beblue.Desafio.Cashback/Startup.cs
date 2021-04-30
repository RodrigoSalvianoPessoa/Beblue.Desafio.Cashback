using System;
using System.Linq;
using Beblue.Desafio.Cashback.Generico.Configuration;
using Beblue.Desafio.Cashback.Generico.Helpers;
using Beblue.Desafio.Cashback.WebApi.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Beblue.Desafio.Cashback
{
    public class Startup
    {
        public IHostEnvironment HostEnvironment { get; private set; }
        public IConfiguration Configuration { get; }
        private readonly SwaggerConfig swaggerConfig;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            HostEnvironment = env;
            Configuration = configuration;
            swaggerConfig = new SwaggerConfig();
            Configuration.GetSection(nameof(SwaggerConfig)).Bind(swaggerConfig);
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddConfiguration(Configuration);
            ConfigureSwagger(services);
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("1.0.0", new OpenApiInfo
                {
                    Version = $"{swaggerConfig.Version}",
                    Title = $"{swaggerConfig.Title}",
                    Description = $"{swaggerConfig.Description}",
                    Contact = new OpenApiContact
                    {
                        Name = $"{swaggerConfig.ContactName}",
                        Url = new Uri(swaggerConfig.ContactUrl),
                        Email = $"{swaggerConfig.ContactEmail}"
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                options.SchemaFilter<EnumSchemaFilter>();
            });

            services.AddBearerTokenAuthorization();
        }

        public class EnumSchemaFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema model, SchemaFilterContext context)
            {
                if (context.Type.IsEnum)
                {
                    model.Enum.Clear();
                    Enum.GetNames(context.Type)
                        .ToList()
                        .ForEach(n => model.Enum.Add(new OpenApiString(n)));
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();


            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = swaggerConfig.RoutePrefix;
                c.SwaggerEndpoint($"{swaggerConfig.Version}/swagger.json", swaggerConfig.Title);
            });
        }
    }
}
