using Microsoft.EntityFrameworkCore;

namespace puc_projeto_eixo_2.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Treino> Treino { get; set; }
    }
}
