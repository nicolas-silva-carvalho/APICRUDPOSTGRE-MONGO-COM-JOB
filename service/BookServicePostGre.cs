using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApi.Models;
using Microsoft.EntityFrameworkCore;
using teste.db;

namespace teste.service
{
    public class BookServicePostGre
    {
        private readonly Context _context;

        public BookServicePostGre(Context context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAsync() =>
        await _context.Books.ToListAsync();

        public async Task<Book?> GetOneAsync(string id) =>
            await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

        public async Task CreateAsync(Book newBook)
        {
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book updatedBook)
        {
            _context.Books.Update(updatedBook);
            await _context.SaveChangesAsync();
        }


        public async Task RemoveAsync(string id)
        {
            Book book = await GetOneAsync(id);
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public async Task JobBook()
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    static string GerarStringAleatoria(int comprimento)
                    {
                        const string caracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                        Random random = new Random();
                        StringBuilder sb = new StringBuilder(comprimento);

                        for (int i = 0; i < comprimento; i++)
                        {
                            int indiceAleatorio = random.Next(caracteresPermitidos.Length);
                            char caractereAleatorio = caracteresPermitidos[indiceAleatorio];
                            sb.Append(caractereAleatorio);
                        }

                        return sb.ToString();
                    }

                    var newBook = new Book
                    {
                        Id = GerarStringAleatoria(4),
                        BookName = "Novo Livro",
                        Price = 150,
                        Category = "Nick",
                        Author = "Autor",
                    };
                    _context.Books.Add(newBook);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw; // Re-lance a exceção para notificar que algo deu errado
                }
            }
        }
    }
}