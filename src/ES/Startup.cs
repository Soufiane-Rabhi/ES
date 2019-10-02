using System;
using System.IO;
using System.Reflection;
using ES.Controllers;
using ES.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;

namespace ES
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Gets application configuration.
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Gets hosting environment.
        /// </summary>
        private IHostingEnvironment Environment { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// 
        /// <param name="configuration">Application configuration.</param>
        /// <param name="environment">Application environment.</param>
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        /// <summary>
        /// Configures application services. This method gets called by the runtime.
        /// </summary>
        ///
        /// <param name="services">Application services container.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsDevelopment())
            {
                // Swagger
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "ES", Version = "v1" });

                    c.IncludeXmlComments($"{Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name)}.xml");
                });
            }

            // Antiforgery
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            // Localization
            services.AddLocalization();

            //HttpClient Factory
            services.AddHttpClient();

            //Infrastructure
            services.AddSerialization();
            services.AddHttpPolicies(Configuration);

            // Mvc
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddJsonOptions(options => options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)
                .AddJsonOptions(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                // Exception Handling
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // Swagger
                app.UseSwagger(options => options.RouteTemplate = "api/{documentName}/spec.json");
                app.UseSwaggerUI(c =>
                {
                    c.DocumentTitle = "ES - Api Explorer";
                    c.RoutePrefix = "api/explorer";
                    c.SwaggerEndpoint("/api/v1/spec.json", "ES API v1");
                });
            }
            else
            {
                // Strict Transport Security
                app.UseHsts();

                // Error Handling
                app.UseStatusCodePagesWithRedirects("/error/{0}");
            }

            // Static Files
            app.UseStaticFiles(new StaticFileOptions { OnPrepareResponse = context => context.Context.Response.Headers.Add("Cache-Control", "public,max-age=31536000") });

            // Authentication
            app.UseAuthentication();

            // Localization
            app.UseRequestLocalization();

            app.UseCors("CorsPolicy");

            // Mvc
            app.UseMvc();
        }
    }
}
