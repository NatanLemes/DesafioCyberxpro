using DesafioBiblioteca.Database;
using DesafioBiblioteca.Dtos;
using DesafioBiblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioBiblioteca.Services.BookServices
{
    public class BookService : IBookInterface
    {
        private readonly DatabaseContext _context;

        public BookService(DatabaseContext context)
        {
            _context = context;
        }





        public async Task<Response<BookDto>> RegistrarLivro(BookDto bookDto)
        {
            Response<BookDto> respostaServico = new Response<BookDto>();

            try
            {
                if (!VerificaLivroExiste(bookDto))
                {
                    respostaServico.Dados = null;
                    respostaServico.status = false;
                    respostaServico.Message = "Livro ja Cadastrado";
                    return respostaServico;
                }

                Book newBook = new Book()
                {
                    Titulo = bookDto.Titulo,
                    SubTitulo = bookDto.SubTitulo,
                    AuthorId = bookDto.AuthorId,
                    Idioma = bookDto.Idioma,
                    DataLancamento = bookDto.DataLancamento,
                    Genero = bookDto.Genero,
                    Editora = bookDto.Editora,
                };

                _context.Livros.Add(newBook);
                await _context.SaveChangesAsync();

                respostaServico.Message = "Livro cadastrado com sucesso";

            }
            catch (Exception ex)
            {
                respostaServico.Dados = null;
                respostaServico.Message = ex.Message;
                respostaServico.status = false;
            }

            return respostaServico;
        }



        public bool VerificaLivroExiste(BookDto bookDto)
        {
            var book = _context.Livros.FirstOrDefault(x => x.Titulo == bookDto.Titulo);

            if (book != null) return false;
            return true;
        }

        public async Task<Response<List<Book>>> BuscaTodosLivros()
        {

            var respostaServico = new Response<List<Book>>();
            try
            {
                var livros = await _context.Livros.ToListAsync();
                if (livros.Count == 0)
                {
                    respostaServico.status = false;
                    respostaServico.Message = "Nenhum Livro Cadastrado";
                    return respostaServico;
                }
                respostaServico.Dados = livros;
            }
            catch (Exception ex)
            {
                respostaServico.Dados = null;
                respostaServico.Message = ex.Message;
                respostaServico.status = false;
            }
            return respostaServico;
        }

        public async Task<Response<Book>> BuscaLivroPorId(int id)
        {
            var respostaServico = new Response<Book>();
            try
            {
                var livro = await _context.Livros.FirstOrDefaultAsync(b => b.Id == id);
                if (livro == null || livro.Id == 0)
                {
                    respostaServico.status = false;
                    respostaServico.Message = "Nenhum Livro Encontrado";
                    return respostaServico;
                }
                respostaServico.Dados = livro;
            }
            catch (Exception ex)
            {
                respostaServico.Dados = null;
                respostaServico.Message = ex.Message;
                respostaServico.status = false;
            }
            return respostaServico;
        }

        public async Task<Response<BookDto>> AlteraLivro(BookDto livroUpdate, int id)
        {
            Response<BookDto> respostaServico = new Response<BookDto>();
            

            try
            {
                var livroBase = await _context.Livros.FirstOrDefaultAsync(b => b.Id == id);

                if (livroBase == null || livroBase.Id == 0)
                {
                    respostaServico.status = false;
                    respostaServico.Message = "Livro informado não encontrado";
                    return respostaServico;
                }

                livroBase.Titulo = livroUpdate.Titulo;
                livroBase.SubTitulo = livroUpdate.SubTitulo;
                livroBase.DataLancamento = livroUpdate.DataLancamento;
                livroBase.AuthorId = livroUpdate.AuthorId;
                livroBase.Idioma = livroUpdate.Idioma;
                livroBase.Editora = livroUpdate.Editora;
                livroBase.Genero = livroUpdate.Genero;
                livroBase.DataLancamento = livroUpdate.DataLancamento;

                await _context.SaveChangesAsync();
                respostaServico.Message = "Livro atualizado com sucesso!";
            }
            catch (Exception ex)
            {
                respostaServico.Dados = null;
                respostaServico.Message = ex.Message;
                respostaServico.status = false;
            }
            return respostaServico;
        }

        public async Task<Response<Book>> DeletaLivro(int id)
        {
            Response<Book> respostaServico = new Response<Book>();

            var livroDel = await _context.Livros.FirstOrDefaultAsync(b => b.Id == id);
            try
            {
                if (livroDel == null || livroDel.Id == 0)
                {
                    respostaServico.status = false;
                    respostaServico.Message = "Livro informado não encontrado";
                    return respostaServico;
                }
                _context.Livros.Remove(livroDel);
                respostaServico.Message = "Livro deletado com sucesso";
                respostaServico.Dados = livroDel;
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



    }
}
