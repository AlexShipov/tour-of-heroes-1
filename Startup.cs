using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace tour_of_heroes_1
{
    /// <summary>
    /// Prevents api get requests from being cached.
    /// </summary>
    public class NoCacheHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public NoCacheHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private static void AddHeader(HttpContext context)
        {
            var headers = context.Response.Headers;

            // if no cache control header add one
            if (!headers.Keys.Contains("Cache-Control"))
            {
                headers.Add(
                    "Cache-Control",
                    new StringValues("no-cache, no-store"));
            }

            // if no pragma add it
            if (!headers.Keys.Contains("Pragma"))
            {
                headers.Add(
                    "Pragma",
                    new StringValues(new[] { "no-cache" }));
            }

            // if pragma exists but doesnt contain no-cahce add it
            if (!headers["Pragma"].Any(item => "no-cache".Equals(item, StringComparison.CurrentCultureIgnoreCase)))
            {
                var pragma = headers["Pragma"];
                var pragmaVal = pragma.ToList();
                pragmaVal.Add("no-cache");
                headers["Pragma"] = new StringValues(pragmaVal.ToArray());
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var isGet = string.Equals(context.Request.Method, "get", StringComparison.CurrentCultureIgnoreCase);

            // if is a get request
            if (isGet)
            {
                var isApi = context
                    .Request.Path.Value
                    .IndexOf(@"api/", StringComparison.CurrentCultureIgnoreCase) != -1;

                // if is an api request
                if (isApi)
                {
                    // add headers on response start
                    context.Response.OnStarting(state =>
                    {
                        AddHeader((HttpContext)state);
                        return Task.FromResult(0);
                    }, context);
                }
            }

            await _next.Invoke(context);
        }
    }

    /// <summary>
    /// Adds custom middleware extensions
    /// </summary>
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseNoCacheHeaderMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NoCacheHeaderMiddleware>();
        }
    }

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            


            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors("AllowAllOrigins");

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseNoCacheHeaderMiddleware();   // use middleware to add headers

            app.UseMvc();
        }
    }
}
