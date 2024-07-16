using DesafioBiblioteca.Database;
using DesafioBiblioteca.Dtos;
using DesafioBiblioteca.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DesafioBiblioteca.Services.UserService
{
	public class UserService : IUserInterface
	{
		private readonly DatabaseContext _context;

		public UserService(DatabaseContext context)
		{
			_context = context;
		}
		public async Task<Response<User>> Cadastrar(UserDto userDto)
		{
			var respostaServico = new Response<User>();
			try
			{
				CriarSenhaHash(userDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

				var novoUsu = new User()
				{
					Usuario = userDto.Usuario,
					SenhaHash = senhaHash,
					SenhaSalt = senhaSalt
				};

				_context.Add(novoUsu);
				await _context.SaveChangesAsync();

				respostaServico.Message = "Usuario Cadastrado com sucesso";
				return respostaServico;
			}
			catch (Exception ex)
			{
				respostaServico.Dados = null;
				respostaServico.Message = ex.Message;
				respostaServico.status = false;
			}

			return respostaServico;
		}

		public void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
		{
			using (HMACSHA512 hmac = new HMACSHA512())
			{
				senhaSalt = hmac.Key;
				senhaHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
			}
		}

		public async Task<Response<User>> Login(LoginDto loginDto)
		{
			var respostaServico = new Response<User>();
			try
			{
				var usuario = await _context.Usuarios.FirstOrDefaultAsync(user => user.Usuario == loginDto.Usuario);
				if (usuario == null)
				{
					respostaServico.Dados = new User();
					respostaServico.Message = "Usuário Invalido!";
					return respostaServico;
				}
				if(!VerificaSenha(loginDto.Senha, usuario.SenhaHash, usuario.SenhaSalt))
				{
					respostaServico.Dados = new User();
					respostaServico.Message = "Senha invalida!";
					return respostaServico;
				}

				respostaServico.Dados = usuario;
				return respostaServico; 
			}
			catch (Exception ex)
			{
				respostaServico.Dados = null;
				respostaServico.Message = ex.Message;
				respostaServico.status = false;
			}

			return respostaServico;
		}
		public bool VerificaSenha(string senha, byte[] senhaHash, byte[] senhaSalt)
		{
			using (var hmac = new HMACSHA512(senhaSalt))
			{
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));

				return computedHash.SequenceEqual(senhaHash);
			}
		}
	}
}
