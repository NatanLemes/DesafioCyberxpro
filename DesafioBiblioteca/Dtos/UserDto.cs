using System.ComponentModel.DataAnnotations;

namespace DesafioBiblioteca.Dtos
{
    public class UserDto
    {
        [Required(ErrorMessage ="Campo Obrigadorio!")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "Campo Obrigadorio!")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Campo Obrigadorio!"), Compare("Senha", ErrorMessage ="As senhas não coincidem")]
        public string ConfirmaSenha { get; set; }
    }
}
