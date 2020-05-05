using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetBookAPI.Options;
using Microsoft.OpenApi.Models;
using TweetBookAPI.Services;
using TweetBookAPI.Authorization;
using Microsoft.AspNetCore.Authorization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using TweetBookAPI.Filters;

namespace TweetBookAPI.Installers
{
    public class MvcOrControllerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);
            services.AddScoped<IIdentityService, IdentityService>();

            //services.AddControllers();
            services
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                    options.Filters.Add<ValidationFilter>();
                })
                .AddFluentValidation(mvcConfiguration=>mvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>())//same like assembly scanning technique used in startup.cs.This will find everything that extend abstract validator class.
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            var tokenValidationParameters= new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(configureOptions: x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddAuthorization(options=>
            {
                options.AddPolicy("MustWorkForABCCompany", policy =>
                {
                    policy.AddRequirements(new WorksForCompanyRequirement("abccompany.com"));
                    //we can add Multiple requirements,claims(AddClaims),Roles(AddRoles) here to restrict on multiple conditions
                });
            });
            services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Tweetbook API", Version = "v1" });
                //----------------------
                //var security = new Dictionary<string, IEnumerable<string>>
                //{
                //    {"Bearer",new string[0] }
                //};
                x.AddSecurityDefinition(name: "Bearer", new OpenApiSecurityScheme
                {
                    Description="JWT Authorization header using the bearer scheme",
                    Name="Authorization",
                    In=ParameterLocation.Header,
                    Type=SecuritySchemeType.ApiKey
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Id="Bearer",
                                Type=ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
                
            });
        }
    }
}
