namespace DesafioBiblioteca.Models
{
    public class Response<T>
    {
        public T? Dados { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool status { get; set; } = true;
    }
}
