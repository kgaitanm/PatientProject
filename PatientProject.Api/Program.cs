
using Microsoft.Extensions.Configuration;
using PatientProject.Api.Filters;
using PatientProject.Application.Interfaces;
using PatientProject.Application.Services;
using PatientProject.Domain.Aggregates.PatientAggregate;
using PatientProject.Domain.Interfaces;
using PatientProject.Infrastructure.Database;
using PatientProject.Infrastructure.Repositories.PatientRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.OpenApi.Models;


namespace PatientProject.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

          

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddSingleton<DatabaseConnection>(DatabaseConnection.CreateInstance(connectionString));
            builder.Services.AddTransient(sp => new EncryptedSocialInsuranceNumber(builder.Configuration));
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<ValidationFilter>();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowPatientManagementApp",
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            builder.Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
            });

            var key = "MyVerySecureSecretKeyWhichIs32BytesLong";
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://localhost:5001",
                        ValidAudience = "https://localhost:5001",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("Token validation failed: " + context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("Token validated successfully");
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Patient Management API", Version = "v1" });
             
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();

            app.UseCors("AllowPatientManagementApp");


          
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient API v1");
                });
            }

            app.UseHttpsRedirection();

      
            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapPost("/token", ([FromBody] TokenRequest request) =>
            {
               
                if (request.ClientId == "PatientManagementClient" && request.ClientSecret == "UGF0aWVudE1hbmFnZW1lbnRDbGllbnQ=")
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes("MyVerySecureSecretKeyWhichIs32BytesLong");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("scope", "api.read"), 
                          
                        }),
                        Expires = DateTime.UtcNow.AddHours(10),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                        Issuer = "https://localhost:5001",
                        Audience = "https://localhost:5001"
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    Console.WriteLine("Generated Token: " + tokenString);

                    return Results.Ok(new { access_token = tokenString });
                }
                return Results.Unauthorized();
            });


            app.Run();
        }

        public record TokenRequest(string ClientId, string ClientSecret);
    }
}
