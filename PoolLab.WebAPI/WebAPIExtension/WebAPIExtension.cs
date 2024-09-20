using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace PoolLab.WebAPI.WebAPIExtension
{
    public static class WebAPIExtension
    {
        public static IServiceCollection AddDIWebAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(filter =>
            {
                filter.SwaggerDoc("v1", new OpenApiInfo
                { Title = "WebAPIv7", Version = "v1" });
                filter.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. " +
                        "\r\n\r\n Enter 'Bearer'[space] and then your token in the " +
                        "text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                filter.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(filter =>
            {
                filter.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = configuration.GetSection("JwtSecurityToken:Audience").Value,
                    ValidIssuer = configuration.GetSection("JwtSecurityToken:Issuer").Value,
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration.GetSection("JwtSecurityToken:Key").Value)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

            return services;
        }
    }
}
