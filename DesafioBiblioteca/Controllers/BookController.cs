using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DesafioBiblioteca.Database;
using DesafioBiblioteca.Models;
using DesafioBiblioteca.Services.BookServices;
using DesafioBiblioteca.Dtos;
using DesafioBiblioteca.Services.AuthorServices;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using DesafioBiblioteca.Filters;
using DesafioBiblioteca.Services.SessionServices;

namespace DesafioBiblioteca.Controllers
{
	[UsuarioLogado]
	public class BookController : Controller
	{
		private readonly IBookInterface _service;
		private readonly IAuthorInterface _author;

		public BookController(IBookInterface service, IAuthorInterface author)
		{
			_service = service;
			_author = author;
		}

		// GET: Book
		public async Task<IActionResult> Index()
		{
			var retorno = _service.BuscaTodosLivros().Result;
			var retornoVB = new Dictionary<int, string>();
			if (retorno.Dados != null && retorno.Dados.Count > 0)
				foreach (var item in retorno.Dados)
				{
					var nome = _author.BuscaAuthorPorId(item.Id).Result.Dados == null ? "Autor Desconhecido" : _author.BuscaAuthorPorId(item.Id).Result.Dados.Nome;

					retornoVB.Add(item.Id, nome);
				}
			ViewBag.LivroEscritor = retornoVB;
			return View(retorno);
		}

		// GET: Book/Details/5
		public async Task<IActionResult> Details(int id)
		{
			var retorno = _service.BuscaLivroPorId(id);
			if (!retorno.Result.status)
			{
				ViewBag.Return = retorno.Result;
				return View(viewName: "Index");
			}
			var nome = _author.BuscaAuthorPorId(retorno.Result.Dados.AuthorId).Result.Dados == null ? "Autor Desconhecido"
                : _author.BuscaAuthorPorId(retorno.Result.Dados.AuthorId).Result.Dados.Nome;

			ViewData["Autor"] = nome;
			return View(retorno.Result.Dados);
		}

		// GET: Book/Create
		public IActionResult Create()
		{
			var retorno = new Response<BookDto>();
			var autores = _author.BuscaTodosAutores().Result;

			// Passe os dados para a view
			if (autores.Dados == null || autores.Dados.Count == 0) return View();
			ViewBag.Autores = new SelectList(autores.Dados, "Id", "Nome");
			ViewBag.Retorno = retorno;
			return View();
		}

		// POST: Book/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Titulo,SubTitulo,AuthorId,Idioma,DataLancamento,Editora,Genero")] BookDto book)
		{
			if (ModelState.IsValid)
			{
				var retorno = await _service.RegistrarLivro(book);
				var autores = _author.BuscaTodosAutores().Result;

				// Passe os dados para a view
				if (autores.Dados == null || autores.Dados.Count == 0) return View();
				ViewBag.Autores = new SelectList(autores.Dados, "Id", "Nome");
				ViewBag.Retorno = retorno;
				return View(book);
			}
			return View(book);
		}

		// GET: Book/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
            var retorno = _service.BuscaLivroPorId(id);
            if (!retorno.Result.status)
            {
                ViewBag.Return = retorno.Result;
                return View(viewName: "Index");
            }
            //var nome = _author.BuscaAuthorPorId(retorno.Result.Dados.AuthorId).Result.Dados == null ? "Autor Desconhecido"
            //    : _author.BuscaAuthorPorId(retorno.Result.Dados.AuthorId).Result.Dados.Nome;

            //ViewData["Autor"] = nome;

            var autores = _author.BuscaTodosAutores().Result;
            // Passe os dados para a view
            if (autores.Dados == null || autores.Dados.Count == 0) return View();
			
            ViewBag.AutoresEdit = new SelectList(autores.Dados, "Id", "Nome");
			ViewBag.RetornoEdit = new Response<BookDto>(); 

            return View(retorno.Result.Dados);
            //return View(nome);
		}

		// POST: Book/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Titulo,SubTitulo,AuthorId,Idioma,DataLancamento,Editora,Genero")] BookDto book)
		{
			if (ModelState.IsValid)
			{
				var retorno = await _service.AlteraLivro(book, id);
				ViewBag.RetornoEdit = retorno;
				return View();
			}
			return RedirectToAction(nameof(Edit), id);
		}

		// GET: Book/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var retorno = _service.BuscaLivroPorId(id);
			if (!retorno.Result.status)
			{
				ViewBag.Return = retorno.Result;
				return View(viewName: "Index");
			}
			var nome = _author.BuscaAuthorPorId(retorno.Result.Dados.AuthorId).Result.Dados == null ? "Autor Desconhecido"
                : _author.BuscaAuthorPorId(retorno.Result.Dados.AuthorId).Result.Dados.Nome;

			ViewData["Autor"] = nome;
            ViewBag.RetornoDel = new Response<BookDto>();
            return View(retorno.Result.Dados);
		}

		// POST: Book/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{

			var retorno = _service.DeletaLivro(id);
            ViewBag.RetornoDel = retorno.Result;

			if (!retorno.Result.status)
				TempData["MensagemErro"] = string.IsNullOrEmpty(retorno.Result.Message)?"Houve um erro" : retorno.Result.Message;
			else
				TempData["MensagemSucesso"] = retorno.Result.Message;
			return RedirectToAction(nameof(Index), retorno.Result);
		}

	}
}
