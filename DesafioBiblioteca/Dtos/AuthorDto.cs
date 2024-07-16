using System.ComponentModel.DataAnnotations;

namespace DesafioBiblioteca.Dtos
{
    public class AuthorDto
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string CPF { get; set; }
        public bool Ativo { get; set; } = true;

    }
}
