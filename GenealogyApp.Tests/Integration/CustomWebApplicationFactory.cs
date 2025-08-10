using GenealogyApp.API;
using GenealogyApp.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace GenealogyApp.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            });


            builder.ConfigureServices(services =>
            {
                // Supprime tous les DbContextOptions<GenealogyDbContext>
                var descriptors = services.Where(
                    d => d.ServiceType == typeof(DbContextOptions<GenealogyDbContext>)).ToList();

                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                // Supprimer le middleware d'authentification
                services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = "fake-token"; // Simule un token
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            // Simule une validation réussie
                            return Task.CompletedTask;
                        }
                    };
                });


                // Ajoute le contexte InMemory
                services.AddDbContext<GenealogyDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Initialise la base
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<GenealogyDbContext>();

                // Nettoyage des données
                db.Users.RemoveRange(db.Users);
                db.SaveChanges();

                db.Database.EnsureCreated();
            });
        }
    }
}
