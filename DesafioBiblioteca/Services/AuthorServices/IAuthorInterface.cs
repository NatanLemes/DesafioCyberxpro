using DesafioBiblioteca.Dtos;
using DesafioBiblioteca.Models;

namespace DesafioBiblioteca.Services.AuthorServices
{
    public interface IAuthorInterface
    {
        Task<Response<AuthorDto>> RegistrarAuthor(AuthorDto authorDto);
        Task<Response<List<Author>>> BuscaTodosAutores();
        Task<Response<Author>> BuscaAuthorPorId(int id);
        Task<int> BuscaQtdLivrosPublicados(int id);

		Task<Response<Author>> AlteraAutor(AuthorDto autorUpdt, int id);
        Task<Response<AuthorDto>> DeletaAuthor(int id);
        Task<Response<AuthorDto>> DeletaAuthorLogico(int id);

    }
}
