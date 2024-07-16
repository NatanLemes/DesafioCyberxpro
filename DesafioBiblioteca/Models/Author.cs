using System.ComponentModel.DataAnnotations;

namespace DesafioBiblioteca.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }
        public bool Ativo { get; set; }
    }
}
