using Architecture_BE.DAL.DataContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Architecture_BE.Business.Services;
using Architecture_BE.DAL.UnitOfWork;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using FluentValidation;
using Architecture_BE.Models.Dto;
using Architecture_BE.Models.Validations;

namespace Architecture_BE.API
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
            #region Regiter Helper

            //Disable Automatic Model State Validation
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation();

            services.AddCors();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #endregion

            #region Register DbContext

            services.AddDbContext<DbArchitectureContext>(options => options.UseSqlServer(
       this.Configuration.GetConnectionString("DefaultConnection")));

            #endregion

            #region Register JWT

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            #endregion

            #region Register Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Probation.API",
                    Description = "Web API using net core.",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header {token}",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });

            #endregion

            #region Register UnitOfWork

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion

            #region Register Business

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();

            #endregion

            #region Register FluentValidation

            #region User 

            services.AddTransient<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddTransient<IValidator<LoginUserDto>, LoginUserDtoValidator>();
            services.AddTransient<IValidator<EditUserDto>, EditUserDtoValidator>();

            #endregion

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Used Services 

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document API V1");
            });

            app.UseMvc();

            #endregion
        }
    }
}
