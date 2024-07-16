using DesafioBiblioteca.Database;
using DesafioBiblioteca.Models;
using DesafioBiblioteca.Services.AuthorServices;
using DesafioBiblioteca.Services.BookServices;
using DesafioBiblioteca.Services.SessionServices;
using DesafioBiblioteca.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddScoped<IBookInterface, BookService>();
builder.Services.AddScoped<IAuthorInterface, AuthorService>();
builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<ISessionInterface, SessionService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();



using (var serviceScope = app.Services.CreateScope())
using (var contexto = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>())
{
    //cadastro reistros padroes
    var authorTeste = new Author { Id = 1, Nome = "Autor 1", DataNascimento = new DateTime(2000,01,01), CPF = "157.754.699-32", Ativo = true};
    contexto.Autores.Add(authorTeste);
    contexto.SaveChanges();
    var livroTeste = new Book { Id = 1, Titulo = "teste", SubTitulo = "teste4", Idioma = "pt", Genero = DesafioBiblioteca.Enums.GeneroEnum.Terror, Editora = "teste", AuthorId = 1, DataLancamento = new DateTime(2000, 01, 01) };
    contexto.Livros.Add(livroTeste);
    contexto.SaveChanges();
    
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
