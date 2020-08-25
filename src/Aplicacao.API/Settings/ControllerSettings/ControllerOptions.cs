using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Aplicacao.API.Settings.SwaggerSettings;
using System.Globalization;

namespace Aplicacao.API.Settings.ControllerSettings
{
    public static class ControllerOptions
    {
        public const string CacheProfileName = "NonAuthoritativeLongDatabaseDuration";

        private const string CorsPolicyName = "CorsPolicy";

        public static void AddServiceCORSAplicacao(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            });
        }

        public static void UseConfigureControllersAplicacao(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            // Extension
            app.UseConfigureSwaggerMITArq();

            var supportedCultures = new[]
{
                new CultureInfo("pt-BR")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseResponseCompression();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}