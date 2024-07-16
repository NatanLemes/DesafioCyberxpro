using DesafioBiblioteca.Enums;
using System.ComponentModel.DataAnnotations;

namespace DesafioBiblioteca.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public int AuthorId { get; set; }
        public string Idioma { get; set; }
        public DateTime DataLancamento { get; set; }
        public string Editora { get; set; }
        public GeneroEnum Genero { get; set; }
    }
}
