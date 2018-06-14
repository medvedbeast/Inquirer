using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using FluentValidation.AspNetCore;
using FluentValidation;
using Inquirer.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using Microsoft.Extensions.WebEncoders;
using System.Text.Unicode;
using System.Text.Encodings.Web;

namespace Inquirer
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
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<InquirerDLL.Validators.UserValidator>();
                    options.ImplicitlyValidateChildProperties = true;
                });

            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = ".inquirer.antiforgery";
                options.Cookie.Expiration = TimeSpan.FromMinutes(15);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.SlidingExpiration = true;
                    options.Cookie.Name = ".inquirer.authorization";
                    options.LoginPath = new PathString("/account/login");
                });

            ValidatorOptions.LanguageManager = new CustomLanguageManager();
            ValidatorOptions.LanguageManager.Culture = new System.Globalization.CultureInfo("uk");

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = false;
            });

            services.AddSession();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });

            services.AddSingleton(HtmlEncoder.Create(new[]
            {
                UnicodeRanges.Cyrillic,
                UnicodeRanges.CyrillicExtendedA,
                UnicodeRanges.CyrillicExtendedB,
                UnicodeRanges.CyrillicSupplement,
                UnicodeRanges.BasicLatin
            }));

            services.AddWebEncoders(options => 
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
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
            application.UseSession(new SessionOptions
            {
                IdleTimeout = TimeSpan.FromMinutes(15),
                Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Expiration = TimeSpan.FromMinutes(15),
                    Name = ".inquirer.session"
                }
            });
            application.UseMvcWithDefaultRoute();
        }
    }
}
