using BookStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using teste.service;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookPostGreController : ControllerBase
{
    private readonly BookServicePostGre _booksService;

    public BookPostGreController(BookServicePostGre booksService)
    {
        _booksService = booksService;
    }
        
    [HttpGet]
    public async Task<List<Book>> Get() =>
        await _booksService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Book>> Get(int id)
    {
        var book = await _booksService.GetOneAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Book newBook)
    {
        await _booksService.CreateAsync(newBook);
        return Ok(newBook);
    }

    private void JobBook(bool erro, int id, DateTime data)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(int id, Book updatedBook)
    {
        var book = await _booksService.GetOneAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedBook.Id = book.Id;

        await _booksService.UpdateAsync(updatedBook);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _booksService.GetOneAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        await _booksService.RemoveAsync(id);

        return NoContent();
    }
}