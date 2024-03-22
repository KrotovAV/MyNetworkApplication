
using MailApp_API_Gateway.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

namespace MailApp_API_Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread.Sleep(3000);
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<IMailRepository, MailRepository>();
            //builder.Services.AddTransient<IUserAuthenticationService, AuthenticationMock>();

            //----------------------
            // было у меня
            //IConfiguration configuration = new ConfigurationBuilder() 
            //                        .AddJsonFile("ocelot.json")
            //                        .Build();
            //было в прримере
            //builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = new ConfigurationBuilder()
                                    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                                    .Build();
            //------------------------------------------------

            builder.Services.AddOcelot(configuration);

            builder.Services.AddSwaggerForOcelot(configuration);

            //--- из примера-----
            //builder.Services
            //    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,

            //            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            //            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
            //        };
            //    });

            //-------------------


            //-----------из user service------

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter tocken",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "Token",

                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                    new string[]{}
                    }
                });
            }
            );
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            //{
            //    opt.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //        ValidAudience = builder.Configuration["Jwt:Audience"],
            //        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

            //        IssuerSigningKey = new RsaSecurityKey(GetPublicKey())
            //    };
            //});
            //----------------------------


            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";

            }).UseOcelot().Wait();


            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}