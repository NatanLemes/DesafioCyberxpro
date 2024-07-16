using DesafioBiblioteca.Dtos;
using DesafioBiblioteca.Models;

namespace DesafioBiblioteca.Services.BookServices
{
    public interface IBookInterface
    {
        Task<Response<BookDto>> RegistrarLivro(BookDto bookDto);
        Task<Response<List<Book>>> BuscaTodosLivros();
        Task<Response<Book>> BuscaLivroPorId(int id);
        Task<Response<BookDto>> AlteraLivro(BookDto livroUpdate, int id);
        Task<Response<Book>> DeletaLivro(int id);
    }
}
