using FluentValidation;
using FluentValidation.AspNetCore;
using InquirerAPI.PublicAPI.Infrastructure;
using InquirerAPI.Website.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace InquirerAPI
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/account/login");
                    options.Cookie.Name = ".inquirer.api.authorization";
                });

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ILogger, Logger>();

            services.AddMvc()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<InquirerDLL.Validators.UserValidator>();
                    options.ImplicitlyValidateChildProperties = true;
                });

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = ".inquirer.api.antiforgery";
            });

            ValidatorOptions.LanguageManager = new CustomLanguageManager()
            {
                Culture = new System.Globalization.CultureInfo("uk")
            };

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
            });

            services.AddRouting(options =>
            {
                options.AppendTrailingSlash = false;
                options.LowercaseUrls = true;
            });

            services.AddDbContext<PublicAPI.Models.DatabaseContext>(options =>
            {
                options.UseMySql(Configuration["Database:Connection:Public API"]);
            });
            services.AddDbContext<Website.Models.DatabaseContext>(options =>
            {
                options.UseMySql(Configuration["Database:Connection:Default"]);
            });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });
        }

        public void Configure(IApplicationBuilder application)
        {
            application.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Environment.ContentRootPath, "node_modules")),
                RequestPath = "/vendor",
                EnableDirectoryBrowsing = false
            });

            application.UseStatusCodePages();
            application.UseDeveloperExceptionPage();
            application.UseStaticFiles();
            application.UseAuthentication();
            application.UseSession();
            application.UseMvc(routes =>
            {
                routes.MapRoute(name: "", template: "{controller=Home}/{action=Index}/{id?}", defaults: new { area = "Website" });
                routes.MapRoute(name: "", template: "public_api/{controller}/{action}/{id?}", defaults: new { area = "PublicAPI" });
                routes.MapRoute(name: "", template: "api/{controller}/{action}/{id?}", defaults: new { area = "API" });
            });
        }
    }
}
