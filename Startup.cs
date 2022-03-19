using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesRegister.ApplicationDbContex;
using Microsoft.OpenApi.Models;
using SalesRegister.HelperClass;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SalesRegister
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

            var sqlConnectionString = Configuration["ConnectionStrings:SalesConnection"];

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseNpgsql(sqlConnectionString)
           );

            services.AddScoped<IFileStorageService, InAppStorageService>();
            services.AddAutoMapper(typeof(Startup));


            services.AddIdentity<StaffModel, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {

                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(Configuration["keyjwt"])),
                            ClockSkew = TimeSpan.Zero

                        };
                    });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAdmin", policy => policy.RequireClaim("role", "admin"));
            });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SalesRegister", Version = "v1" });
          //      //?? new code
          //      var securitySchema = new OpenApiSecurityScheme
          //      {
          //          Description = "Using the Authorization header with the Bearer scheme.",
          //          Name = "Authorization",
          //          In = ParameterLocation.Header,
          //          Type = SecuritySchemeType.Http,
          //          Scheme = "bearer",
          //          Reference = new OpenApiReference
          //          {
          //              Type = ReferenceType.SecurityScheme,
          //              Id = "Bearer"
          //          }
          //      };

          //      c.AddSecurityDefinition("Bearer", securitySchema);

          //      c.AddSecurityRequirement(new OpenApiSecurityRequirement
          //{
          //    { securitySchema, new[] { "Bearer" } }
          //});
          //      //?? new code
            });

            services.AddCors(options =>
            {
                var frontendURL = Configuration.GetValue<string>("frontend_url");
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader()
                        .WithExposedHeaders(new string[] { "totalAmountOfRecords" });
                });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dataContext)
        {
            dataContext.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SalesRegister v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
