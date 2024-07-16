using DesafioBiblioteca.Dtos;
using DesafioBiblioteca.Filters;
using DesafioBiblioteca.Models;
using DesafioBiblioteca.Services.AuthorServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DesafioBiblioteca.Controllers
{
    [UsuarioLogado]
    public class AuthorController : Controller
    {

        private readonly IAuthorInterface _authorInterface;
        public AuthorController(IAuthorInterface authorInterface)
        {
            _authorInterface = authorInterface;
        }

        //getall
        public async Task<IActionResult> Index()
        {
            var retorno = await _authorInterface.BuscaTodosAutores();
            if (retorno.Dados.Any())
                return View(retorno.Dados);
            else
                TempData["MensagemErro"] = retorno.Message;

            return View();
        }
        //getbyid
		public async Task<IActionResult> Details(int id)
		{
			var retorno = await _authorInterface.BuscaAuthorPorId(id);
			if (retorno.Dados!= null)
            {
                TempData["QtdLivros"] = await _authorInterface.BuscaQtdLivrosPublicados(id);
				return View(retorno.Dados);
            }
			else
				TempData["MesagemErro"] = retorno.Message;

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Create()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Create(AuthorDto authorDto)
        {
            if (ModelState.IsValid)
            {
                var retorno = await _authorInterface.RegistrarAuthor(authorDto);
                if (!retorno.status)
                {
                    TempData["MensagemErro"] = retorno.Message;
                    return View(authorDto);
                }
                TempData["MensagemSucesso"] = retorno.Message;
                return View(authorDto);
            }
            else
                return View(authorDto);
		}

        public async Task<IActionResult> Edit(int id)
        {
			var retorno = await _authorInterface.BuscaAuthorPorId(id);
			if (retorno.Dados != null)
				return View(retorno.Dados);
			else
				TempData["MesagemErro"] = retorno.Message;

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, AuthorDto autorDto)
		{
			if (ModelState.IsValid)
			{
				var retorno = await _authorInterface.AlteraAutor(autorDto, id);
				if (!retorno.status)
				{
					TempData["MensagemErro"] = retorno.Message;
					return View(retorno.Dados);
				}
				TempData["MensagemSucesso"] = retorno.Message;
				return View(retorno.Dados);
			}
			else
				return View(new Author { Nome = autorDto.Nome, CPF = autorDto.CPF, DataNascimento = autorDto.DataNascimento, Id = id});
		}

		public async Task<IActionResult> Delete(int id)
		{
			var retorno = await _authorInterface.BuscaAuthorPorId(id);
			if (!retorno.status)
			{
				TempData["MensagemErro"] = retorno.Message; 
				return View(viewName: "Index");
			}
			TempData["QtdLivros"] = await _authorInterface.BuscaQtdLivrosPublicados(id);
			return View(retorno.Dados);
		}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var retorno =await  _authorInterface.DeletaAuthorLogico(id);

            if (!retorno.status)
                TempData["MensagemErro"] = string.IsNullOrEmpty(retorno.Message) ? "Houve um erro" : retorno.Message;
            else
                TempData["MensagemSucesso"] = retorno.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
