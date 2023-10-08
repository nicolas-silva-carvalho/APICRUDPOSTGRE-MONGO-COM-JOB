using System.Text.Json;
using BookStoreApi.Models;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using teste.db;
using teste.model;

namespace teste.service
{
    public class BookServicePostGre
    {
        private readonly Context _context;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public BookServicePostGre(Context context, IBackgroundJobClient backgroundJobClient)
        {
            _context = context;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<List<Book>> GetAsync() =>
        await _context.Books.ToListAsync();

        public async Task<Book?> GetOneAsync(int id) =>
            await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Book> CreateAsync(Book newBook)
        {
            try
            {
                var x = _context.Books.Add(newBook);
                var t = await _context.SaveChangesAsync();
                string json = null;

                if (t > 0)
                {
                    // Se o salvamento no banco de dados foi bem-sucedido, crie um objeto anônimo
                    var successObject = new
                    {
                        Success = true,
                        Message = "Livro inserido com sucesso",
                        BookId = newBook.Id
                    };

                    // Serializar o objeto em JSON
                    json = JsonSerializer.Serialize(successObject);
                }
                else
                {
                    // Se o salvamento no banco de dados não foi bem-sucedido, crie um objeto anônimo com uma mensagem de erro
                    var errorObject = new
                    {
                        Success = false,
                        Message = "Falha ao inserir o livro no banco de dados"
                    };

                    // Serializar o objeto em JSON
                    json = JsonSerializer.Serialize(errorObject);
                }

                _backgroundJobClient.Enqueue(() => SaveResultJob(json, newBook.Id, DateTime.UtcNow));

                return newBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SaveResultJob(string v, int id, DateTime utcNow)
        {
            var t = new BookContrato
            {
                Erro = v,
                DateTime = utcNow,
                IdBook = id
            };

            _context.bookContratos.Add(t);
            _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book updatedBook)
        {
            _context.Books.Update(updatedBook);
            await _context.SaveChangesAsync();
        }


        public async Task RemoveAsync(int id)
        {
            Book book = await GetOneAsync(id);
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        // public async Task JobBook()
        // {
        //     using (var transaction = await _context.Database.BeginTransactionAsync())
        //     {
        //         try
        //         {
        //             static string GerarStringAleatoria(int comprimento)
        //             {
        //                 const string caracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        //                 Random random = new Random();
        //                 StringBuilder sb = new StringBuilder(comprimento);

        //                 for (int i = 0; i < comprimento; i++)
        //                 {
        //                     int indiceAleatorio = random.Next(caracteresPermitidos.Length);
        //                     char caractereAleatorio = caracteresPermitidos[indiceAleatorio];
        //                     sb.Append(caractereAleatorio);
        //                 }

        //                 return sb.ToString();
        //             }

        //             var newBook = new Book
        //             {
        //                 Id = GerarStringAleatoria(4),
        //                 BookName = "Novo Livro",
        //                 Price = 150,
        //                 Category = "Nick",
        //                 Author = "Autor",
        //             };
        //             _context.Books.Add(newBook);

        //             await _context.SaveChangesAsync();
        //             await transaction.CommitAsync();
        //         }
        //         catch (Exception)
        //         {
        //             await transaction.RollbackAsync();
        //             throw; // Re-lance a exceção para notificar que algo deu errado
        //         }
        //     }
        // }
    }
}