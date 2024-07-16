using DesafioBiblioteca.Models;

namespace DesafioBiblioteca.Services.SessionServices
{
	public interface ISessionInterface
	{
		User BuscaSessao();
		void CriarSessao(User user);
		void RemoverSessao();
	}
}
