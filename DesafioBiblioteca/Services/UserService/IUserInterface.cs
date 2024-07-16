using DesafioBiblioteca.Dtos;
using DesafioBiblioteca.Models;

namespace DesafioBiblioteca.Services.UserService
{
    public interface IUserInterface
    {
       Task<Response<User>> Cadastrar(UserDto userDto);
       Task<Response<User>> Login(LoginDto userDto);
	}
}
