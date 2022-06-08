using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Message.Models;
using Message.Helpers;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Message.Solution
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

        
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Messageboard API",
                    Description = "API for retrieving posts from threads",
                    Contact = new OpenApiContact
                    {
                        Name = "Cameron Ray",
                        Email = "Cameron@cameronLRay.com",
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddDbContext<MessageContext>(options =>
            {
                options.UseMySql(Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(Configuration["ConnectionStrings:DefaultConnection"]));
                options.EnableSensitiveDataLogging();

            });
                
            services.AddControllers();

            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<MessageContext>()
            .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
            });

            // services.AddDbContext<MessageDbContext>(options =>
            //         options.UseSqlServer(Configuration.GetConnectionString("MessageDbContext")));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CretaceousPark API V");
                c.RoutePrefix = string.Empty;
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
