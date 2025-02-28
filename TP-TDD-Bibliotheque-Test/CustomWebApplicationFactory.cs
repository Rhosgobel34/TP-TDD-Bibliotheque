using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TP_TDD_Bibliotheque;

namespace TP_TDD_Bibliotheque_Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(Services =>
            {
                var descriptor = Services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null)
                {
                    Services.Remove(descriptor);
                }

                string connectionString = "Server=host.docker.internal;Port=3306;Database=bibliotheque;User=api-biblio;Password=bibliothecaire";
                Services.AddDbContext<AppDbContext>(option =>
                {
                    option.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                });
            });
        }
    }
}
