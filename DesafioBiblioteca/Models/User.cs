using System.ComponentModel.DataAnnotations;

namespace DesafioBiblioteca.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Usuario { get; set; }
        public byte[] SenhaHash { get; set; }
        public byte[] SenhaSalt { get; set; }
    }
}
