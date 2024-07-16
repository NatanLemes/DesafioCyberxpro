using DesafioBiblioteca.Dtos;
using DesafioBiblioteca.Models;
using DesafioBiblioteca.Services.SessionServices;
using DesafioBiblioteca.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBiblioteca.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserInterface _userInterface;
        private readonly ISessionInterface _sessionInterface;

		public UserController(IUserInterface userInterface, ISessionInterface sessionInterface)
		{
			_userInterface = userInterface;
			_sessionInterface = sessionInterface;
		}
		public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<User>> Cadastrar(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var retorno = await _userInterface.Cadastrar(userDto);

                if (retorno.status)
                {
                    TempData["MensagemSucesso"] = retorno.Message;
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    TempData["MesagemErro"] = retorno.Message;
                    return View(userDto);
                }
            }
            else
            {
                return View(userDto);
            }
        }

		public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
        public async Task<ActionResult<User>> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var usu = await _userInterface.Login(loginDto);

                if(usu.Dados.Id == 0)
                {
                    TempData["MensagemErro"] = usu.Message;
                    return View(loginDto);
                }
                else
                {
					TempData["MensagemSucesso"] = usu.Message;
                    _sessionInterface.CriarSessao(usu.Dados);
					return RedirectToAction("Index", "Home");
				}
            }
            else
            {
                return View(loginDto);
            }
        }

        public IActionResult Sair()
        {
            _sessionInterface.RemoverSessao();
            return RedirectToAction("Login", "User");
        }
    }
}
