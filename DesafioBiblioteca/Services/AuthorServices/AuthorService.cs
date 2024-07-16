using DesafioBiblioteca.Database;
using DesafioBiblioteca.Dtos;
using DesafioBiblioteca.Models;
using DesafioBiblioteca.Utils;
using Microsoft.EntityFrameworkCore;

namespace DesafioBiblioteca.Services.AuthorServices
{
	public class AuthorService : IAuthorInterface
	{
		private readonly DatabaseContext _context;

		public AuthorService(DatabaseContext context)
		{
			_context = context;
		}


		public async Task<Response<AuthorDto>> RegistrarAuthor(AuthorDto authorDto)
		{
			Response<AuthorDto> respostaServico = new Response<AuthorDto>();


			try
			{
				if (!ValidaCpf.IsCpf(authorDto.CPF))
				{
					respostaServico.Dados = null;
					respostaServico.status = false;
					respostaServico.Message = "CPF Invalido";
					return respostaServico;
				}

				if (!VerificaAuthorExiste(authorDto))
				{
					if (this.AtivaAutor(authorDto.CPF))
					{
						respostaServico.Dados = null;
						respostaServico.status = true;
						respostaServico.Message = "Autor com informações encontradas, cadastro reativado.";
						return respostaServico;
					}
					respostaServico.Dados = null;
					respostaServico.status = false;
					respostaServico.Message = "Autor ja existente";

					return respostaServico;
				}

				Author autor = new Author()
				{
					Nome = authorDto.Nome,
					CPF = authorDto.CPF,
					DataNascimento = authorDto.DataNascimento,
					Ativo = true
				};

				_context.Autores.Add(autor);
				await _context.SaveChangesAsync();

				respostaServico.Message = "Autor cadastrado com sucesso";

			}
			catch (Exception ex)
			{
				respostaServico.Dados = null;
				respostaServico.Message = ex.Message;
				respostaServico.status = false;
			}

			return respostaServico;
		}

		public bool VerificaAuthorExiste(AuthorDto authorDto)
		{
			var autorBusca = _context.Autores.FirstOrDefault(x => x.CPF == authorDto.CPF);

			if (autorBusca != null) return false;
			return true;
		}

		public async Task<Response<List<Author>>> BuscaTodosAutores()
		{

			var respostaServico = new Response<List<Author>>();
			try
			{
				var autores = await _context.Autores.Where(x => x.Ativo == true).ToListAsync();
				if (autores.Count == 0)
				{
					respostaServico.Dados = new List<Author>();
					respostaServico.status = false;
					respostaServico.Message = "Nenhum autor Cadastrado";
					return respostaServico;
				}
				respostaServico.Dados = autores;
			}
			catch (Exception ex)
			{
				respostaServico.Dados = new List<Author>();
				respostaServico.Message = ex.Message;
				respostaServico.status = false;
			}
			return respostaServico;
		}


		public async Task<Response<Author>> BuscaAuthorPorId(int id)
		{
			var respostaServico = new Response<Author>();
			try
			{
				var autor = await _context.Autores.FirstOrDefaultAsync(b => b.Id == id);
				if (autor == null || autor.Id == 0)
				{
					respostaServico.status = false;
					respostaServico.Message = "Nenhum autor Encontrado";
					return respostaServico;
				}
				respostaServico.Dados = autor;
			}
			catch (Exception ex)
			{
				respostaServico.Dados = null;
				respostaServico.Message = ex.Message;
				respostaServico.status = false;
			}
			return respostaServico;
		}

		public async Task<int> BuscaQtdLivrosPublicados(int id)
		{
			var qtdLivros = await _context.Livros.CountAsync(x => x.AuthorId == id);
			return qtdLivros;
		}

		public async Task<Response<Author>> AlteraAutor(AuthorDto autorUpdt, int id)
		{
			Response<Author> respostaServico = new Response<Author>();


			try
			{
				var autorBase = await _context.Autores.FirstOrDefaultAsync(b => b.Id == id && b.Ativo == true);

				if (!ValidaCpf.IsCpf(autorUpdt.CPF))
				{
					respostaServico.Dados = null;
					respostaServico.status = false;
					respostaServico.Message = "CPF Invalido";
					return respostaServico;
				}

				if (autorBase == null || autorBase.Id == 0)
				{
					respostaServico.status = false;
					respostaServico.Message = "Autor não encontrado";
					return respostaServico;
				}

				autorBase.Nome = autorUpdt.Nome;
				autorBase.CPF = autorUpdt.CPF;
				autorBase.DataNascimento = autorUpdt.DataNascimento;
				autorBase.Ativo = autorUpdt.Ativo;
				respostaServico.Dados = autorBase;
				respostaServico.Message = "Registro alterado com sucesso";

				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				respostaServico.Dados = null;
				respostaServico.Message = ex.Message;
				respostaServico.status = false;
			}
			return respostaServico;
		}


		public async Task<Response<AuthorDto>> DeletaAuthor(int id)
		{
			Response<AuthorDto> respostaServico = new Response<AuthorDto>();

			try
			{
				var autorDel = await _context.Autores.FirstOrDefaultAsync(b => b.Id == id && b.Ativo == true);
				if (autorDel == null || autorDel.Id == 0)
				{
					respostaServico.status = false;
					respostaServico.Message = "Autor não encontrado";
					return respostaServico;
				}
				_context.Autores.Remove(autorDel);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				respostaServico.Dados = null;
				respostaServico.Message = ex.Message;
				respostaServico.status = false;
			}
			return respostaServico;

		}


		public async Task<Response<AuthorDto>> DeletaAuthorLogico(int id)
		{
			Response<AuthorDto> respostaServico = new Response<AuthorDto>();

			try
			{
				var autorDel = await _context.Autores.FirstOrDefaultAsync(b => b.Id == id && b.Ativo == true);
				if (autorDel == null || autorDel.Id == 0)
				{
					respostaServico.status = false;
					respostaServico.Message = "Autor não encontrado";
					return respostaServico;
				}
				autorDel.Ativo = false;
				respostaServico.Message = "Autor deletado";
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				respostaServico.Dados = null;
				respostaServico.Message = ex.Message;
				respostaServico.status = false;
			}
			return respostaServico;

		}

		public bool AtivaAutor(string cpf)
		{
			var retorno = true;

			try
			{
				var autorDel = _context.Autores.FirstOrDefault(x => x.CPF == cpf && x.Ativo == false);
				if (autorDel == null || autorDel.Id == 0)
				{
					retorno = false;
					return retorno;
				}
				autorDel.Ativo = true;
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				retorno = false;
			}
			return retorno;

		}
	}
}
