using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Aplicacao.API.Settings.SwaggerSettings
{
    public class CustomSwaggerDocumentAttribute : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Info = new OpenApiInfo
            {
                Title = "Infnet | MIT Arquitetura de Software",
                Version = "v1",
                Description = "Projeto sobre uso de cache em API usando Redis",
                TermsOfService = new Uri("https://www.infnet.com.br"),
                Contact = new OpenApiContact
                {
                    Name = "Felipe Augusto",
                    Email = "felipementel@hotmail.com"
                },
                License = new OpenApiLicense
                {
                    Name = "BSD",
                    Url = new Uri("https://pt.wikipedia.org/wiki/Licen%C3%A7a_BSD"),
                }
            };
        }
    }
}