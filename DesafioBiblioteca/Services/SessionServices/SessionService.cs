using DesafioBiblioteca.Models;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace DesafioBiblioteca.Services.SessionServices
{
	public class SessionService : ISessionInterface
	{
		private readonly IHttpContextAccessor _httpAccessor;
		public SessionService(IHttpContextAccessor httpAcessor)
		{
			_httpAccessor = httpAcessor;
		}

		public User BuscaSessao()
		{
			string sessaoUsuario = _httpAccessor.HttpContext.Session.GetString("UsuarioAtivo");
			if (sessaoUsuario == null)
				return null;

			return JsonConvert.DeserializeObject<User>(sessaoUsuario);
		}

		public void CriarSessao(User user)
		{
			string usuarioJson = JsonConvert.SerializeObject(user);
			_httpAccessor.HttpContext.Session.SetString("UsuarioAtivo", usuarioJson);
		}

		public void RemoverSessao()
		{
			_httpAccessor.HttpContext.Session.Remove("UsuarioAtivo");
		}
	}
}
