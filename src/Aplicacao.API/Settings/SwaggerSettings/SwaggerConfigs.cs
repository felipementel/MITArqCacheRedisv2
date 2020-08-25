using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Aplicacao.API.Settings.SwaggerSettings
{
    public static class SwaggerConfigs
    {
        public static void AddSwaggerConfigAplicacao(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<CustomSwaggerDocumentAttribute>();
                c.OperationFilter<CustomHeaderSwaggerAttribute>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header | Bearer Schema. <br>
                      Informe: Bearer [espaço] o token obtido na controller de login <br>
                      Exemplo: Bearer 12345abcdef",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.CustomSchemaIds(i => i.FullName);
            });
        }

        /// <summary>
        /// Configura o Swagger
        /// </summary>
        /// <param name="app">Aplicação</param>
        public static void UseConfigureSwaggerMITArq(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Infnet | MIT Arquitetura de Software");
            });
        }
    }
}
