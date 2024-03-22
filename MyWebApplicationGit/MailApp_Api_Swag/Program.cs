using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace MainMailApiMultiSwagger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread.Sleep(3000);
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            //builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            //builder.Services.AddTransient<IUserAuthenticationService, UserAuthenticationService>();

            builder.Services.AddSwaggerGen(opt =>
            {
                //opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //In = ParameterLocation.Header,
                //Description = "Please enter tocken",
                //Name = "Authorization",
                //    Type = SecuritySchemeType.Http,
                //    BearerFormat = "Token",

                //    Scheme = "bearer"
                //});

                opt.SwaggerDoc("logInOut", new OpenApiInfo
                {
                    Title = "LogInOut",
                    Version = "v1",
                    //Description = "logInOut",
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "Пример контакта",
                    //    Url = new Uri("https://example.com/contact")
                    //},
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Пример лицензии",
                    //    Url = new Uri("https://example.com/license")
                    //}
                });
                opt.SwaggerDoc("registration", new OpenApiInfo
                {
                    Title = "Registration",
                    Version = "v1",
                    //Description = "Registration administrator",
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "Пример контакта",
                    //    Url = new Uri("https://example.com/contact")
                    //},
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Пример лицензии",
                    //    Url = new Uri("https://example.com/license")
                    //}
                });
                opt.SwaggerDoc("mail", new OpenApiInfo
                {
                    Title = "Mail",
                    Version = "v1",
                    //Description = "The main information",
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "Пример контакта",
                    //    Url = new Uri("https://example.com/contact")
                    //},
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Пример лицензии",
                    //    Url = new Uri("https://example.com/license")
                    //}
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
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            var app = builder.Build();
            app.UseSession();
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });

            app.UseSession();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                //app.UseSwaggerUI();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/registration/swagger.json", "RegistrationService");
                    c.SwaggerEndpoint("/swagger/logInOut/swagger.json", "LogInOutService");
                    c.SwaggerEndpoint("/swagger/mail/swagger.json", "MailService");
                });

            }
            
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}