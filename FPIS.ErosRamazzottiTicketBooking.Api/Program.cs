using FPIS.ErosRamazzottiTicketBooking.Api.Middleware;
using FPIS.Infrastructure;
using FPIS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FPIS.ErosRamazzottiTicketBooking.Api
{
    /// <summary>
    /// Ulazna tačka aplikacije; konfiguriše DI kontejner, middleware pipeline i pokreće web host.
    /// </summary>
    public class Program
    {
        /// <summary>Glavna metoda; konfiguriše i pokreće web aplikaciju.</summary>
        /// <param name="args">Argumenti komandne linije.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var allowedOrigins = "AllowedOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: allowedOrigins,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Eros Ramazzotti Ticket Booking API", Version = "v1" });

                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "API Key needed to access the endpoints. X-Api-Key: {apiKey}",
                    In = ParameterLocation.Header,
                    Name = "X-Api-Key",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKeyScheme"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                        });

            var assembly = typeof(Program).Assembly;
            builder.Services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.InNamespaces("FPIS.ErosRamazzottiTicketBooking.Api.Services"))
                .AsMatchingInterface()
                .WithScopedLifetime()
            );

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(allowedOrigins); // CORS before custom/auth middleware

            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
