using DesafioBiblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioBiblioteca.Database
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "BibliotecaDb");
            optionsBuilder.EnableSensitiveDataLogging();

        }

        public DbSet<Book> Livros { get; set; }
        public DbSet<Author> Autores { get; set; }

        public DbSet<User> Usuarios { get; set; }
    }
}
