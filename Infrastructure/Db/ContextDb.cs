using Microsoft.EntityFrameworkCore;
using MINIMALAPI.Domain.Entities;

namespace MINIMALAPI.Infrastructure.Db
{
    public class ContextDb : DbContext
    {
        private readonly IConfiguration _configurationAppSettings;
        public ContextDb(IConfiguration configurationAppSettings)
        {
            _configurationAppSettings = configurationAppSettings;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>().HasData(
                new Administrator
                {
                    Id = 1,
                    Email = "admin@admin.com",
                    Perfil = "Adm",
                    Senha = "admin"
                }
            );
        }
        public DbSet<Administrator> Administrators { get; set; } = default!;
        public DbSet<Veiculo> Veiculos { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var string_connection = _configurationAppSettings.GetConnectionString("Mysql")?.ToString();
                if (!string.IsNullOrEmpty(string_connection)){
                    optionsBuilder.UseMySql(string_connection,
                    ServerVersion.AutoDetect(string_connection));
                }
            }
        }

    }
}