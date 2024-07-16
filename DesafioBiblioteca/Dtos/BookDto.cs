using DesafioBiblioteca.Enums;
using DesafioBiblioteca.Models;
using System.ComponentModel.DataAnnotations;

namespace DesafioBiblioteca.Dtos
{
    public class BookDto
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Idioma { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime DataLancamento { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Editora { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public GeneroEnum Genero { get; set; }
    }
}
