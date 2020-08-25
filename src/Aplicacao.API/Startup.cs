using Aplicacao.API.Settings.ControllerSettings;
using Aplicacao.API.Settings.SwaggerSettings;
using Aplicacao.Infra.CrossCutting;
using Aplicacao.Infra.DataAccess.Context;
using AutoMapper;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace Aplicacao.API
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(opt =>
                {
                    var serializerOptions = opt.JsonSerializerOptions;
                    serializerOptions.IgnoreNullValues = true;
                    serializerOptions.IgnoreReadOnlyProperties = false;
                    serializerOptions.WriteIndented = true;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var result = new BadRequestObjectResult(context.ModelState);

                        result.ContentTypes.Add(MediaTypeNames.Application.Json);
                        //result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                        return result;
                    };
                });
            //.AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //});

            if (Environment.IsDevelopment())
            {
                services.AddControllers(opts =>
                {
                    opts.Filters.Add(new AllowAnonymousFilter());
                });
            }

            services.Configure<GzipCompressionProviderOptions>(
                options => options.Level = System.IO.Compression.CompressionLevel.Optimal);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            services.AddDbContext<DbContext, AplicacaoContext>(opt => opt
                .UseSqlServer(Configuration.GetConnectionString("Aplicacao"), options =>
                {
                    options.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }
                )
                .EnableSensitiveDataLogging(Environment.IsDevelopment())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll));


            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "MITArq-";
            });

            //https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/configuration/options?view=aspnetcore-3.1
            services.AddOptions();

            services.AddAutoMapper(typeof(Startup));

            // Extension
            services.AddSwaggerConfigAplicacao();

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
                o.ApiVersionReader = new HeaderApiVersionReader("x-api-aplicacao");
            });

            // Extension
            services.AddServiceCORSAplicacao();

            // Extension
            services.AddRegisterServicesAplicacao();

            Injections.SetSecurity(services, Configuration);

            services.AddApplicationInsightsTelemetry();

            services.ConfigureTelemetryModule<QuickPulseTelemetryModule>((module, o) =>
            {
                module.AuthenticationApiKey = Configuration.GetValue("ApplicationInsights:AuthenticationApiKey", string.Empty);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IHostApplicationLifetime lifetime,
            IDistributedCache cache, DbContext dataContext)
        {
            //dataContext.Database.EnsureCreated();
            //dataContext.Database.Migrate();

            //lifetime.ApplicationStarted.Register(() =>
            //{
            //    var currentTimeUTC = DateTime.UtcNow.ToString();
            //    byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);

            //    var options = new DistributedCacheEntryOptions()
            //        .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            //    cache.Set("cachedTimeUTC", encodedCurrentTimeUTC, options);
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(
                    options =>
                    {
                        options.Run(
                            async context =>
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                context.Response.ContentType = "text/html";
                                var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();
                                if (null != exceptionObject)
                                {
                                    var errorMessage = $"<b>Error: {exceptionObject.Error.Message}</b> { exceptionObject.Error.StackTrace}";
                                    await context.Response.WriteAsync(errorMessage).ConfigureAwait(false);
                                }
                            });
                    }
                    );
            }

            // Extension
            app.UseConfigureControllersAplicacao();
        }
    }
}
