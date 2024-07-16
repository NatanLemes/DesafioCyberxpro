using System.ComponentModel.DataAnnotations;

namespace DesafioBiblioteca.Dtos
{
	public class LoginDto
	{
        [Required(ErrorMessage ="Campo Obrigatório")]
        public string Usuario { get; set; }
		[Required(ErrorMessage = "Campo Obrigatório")]
		public string Senha { get; set; }
    }
}
