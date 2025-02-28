using Microsoft.AspNetCore.Mvc;
using TP_TDD_Bibliotheque.Models;

namespace TP_TDD_Bibliotheque.Controllers;

[ApiController]
[Route("/book")]
public class BookController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] Book book)
    {
        if (book == null) return BadRequest("Le livre est invalide");

        var editor = await _context.Editors.FindAsync(book.EditorId);
        if (editor == null) return BadRequest("L'éditeur n'existe pas");

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }
}
