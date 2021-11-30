using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Authentication;
using AuthMicroservice.Core.Fluent;
using AuthMicroservice.Core.Fluent.Entities;
using AuthMicroservice.Core.Interfaces.Services;
using AuthMicroservice.Core.Middlewares;
using AuthMicroservice.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AuthMicroservice
{
    public class Startup
    {
        private bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Authentication
            services.Configure<ApplicationOptions>(Configuration.GetSection("ApplicationOptions"));

            var authenticationSettings = new AuthenticationSettings();
            Configuration.GetSection("Authentication").Bind(authenticationSettings);
            services.AddSingleton(authenticationSettings);

            services.AddScoped<IHeaderContextService, HeaderContextService>();

            services.AddScoped<IJwtCookieService, JwtCookieService>();

            services.AddHttpContextAccessor();
			
			services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
            });
            #endregion 

            services.AddDbContext<MicroserviceContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), builder => {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });

            services.AddControllers();
            services.AddScoped<IPasswordHasher<UserDomain>, PasswordHasher<UserDomain>>();

            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddAutoMapper(this.GetType().Assembly);

            #region swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EDP-AUTH-MSV", Version = "v1" });
            });
            #endregion

            services.AddScoped<IEnterpriseService, EnterpriseService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IMicroserviceService, MicroserviceService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EDP-AUTH-MSV");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();
           
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
