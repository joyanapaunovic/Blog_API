using Blog.Application.UseCases.Queries;
using Blog.DataAccess;
using Blog.Implementation;
using Blog.Implementation.UseCases.Queries.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Implementation.Validators;
using Blog.API.Core;
using Blog.API.Extensions;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Blog.Domain;
using Microsoft.AspNetCore.Http;
using Blog.Application.UseCases;
using Blog.Implementation.UseCases.UseCaseLoggers;
using Blog.API.Middleware;
using Blog.Implementation.UseCases.Commands.EF.Blog_posts;
using Blog.Application.UseCases.Commands.Blog_posts;
using Blog.API.Controllers;
using FluentValidation;
using Blog.Implementation.Logging;

namespace Blog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settings = new AppSettings();
            // Login validator:
            //services.AddScoped<IValidator<TokenController.TokenRequest>, LoginUserValidator>();
            Configuration.Bind(settings);
            services.AddSingleton(settings);
            services.AddControllers();
            // Jwt, UseCases, IApplicationUser
            services.AddJwt(settings);
            services.AddTransient<IDeleteBlogPostCommand, EFDeleteBlogPostCommand>();
            services.AddUseCases();
            // IApplicationUser
            services.AddApplicationUser();
            services.AddHttpContextAccessor();
            // DB CONTEXT
            services.AddDbContext<BlogContext>(options => 
            options.UseSqlServer("Data Source=DESKTOP-CIO2VEQ;Initial Catalog=blog;Integrated Security=True"));
            
            // localhost:5000/swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog.API", Version = "v1" });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
       

            // LOGGING EXCEPTIONS
            services.AddTransient<Blog.Application.Logging.IExceptionLogger, Blog.Implementation.Logging.ConsoleExceptionLogger>();
            // ISPIS U KONZOLI I POTOM SLANJE U BAZU PODATAKA!
            services.AddTransient<IUseCaseLogger, ConsoleUseCaseLogger>();
            // USE CASE HANDLER
            services.AddTransient<UseCaseHandler>();
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog.API v1"));
            }

            app.UseRouting();
            // middleware
            
            app.UseAuthentication();
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
