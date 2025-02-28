using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        if (string.IsNullOrEmpty(book.ISBN) || string.IsNullOrEmpty(book.Title))
        {
            return BadRequest("Le livre est invalide");
        }

        if (book.ISBN.Length == 13)
        {
            if (!CheckISBN13(book.ISBN))
            {
                return BadRequest("L'ISBN-13 est invalide");
            }
        }
        else if (book.ISBN.Length == 10)
        {
            if (!CheckISBN10(book.ISBN))
            {
                return BadRequest("L'ISBN-10 est invalide");
            }
        }
        else
        {
            return BadRequest("L'ISBN n'a pas un taille valide");
        }

        if (await _context.Books.AnyAsync(b => b.ISBN == book.ISBN))
        {
            return BadRequest("Le livre existe déjà");
        }

        if (book.EditorId == 0)
        {
            return BadRequest("L'éditeur est invalide");
        }

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

    private static bool CheckISBN13(string isbn)
    {
        int total = 0;
        for (int i = 0; i < 13; i++)
        {
            int digit = isbn[i] - '0';

            if (i % 2 == 0)
            {
                total += digit;
            }
            else
            {
                total += digit * 3;
            }
        }

        return total % 10 == 0;
    }

    private static bool CheckISBN10(string isbn)
    {
        int total = 0;
        for (int i = 0; i < 10; i++)
        {
            if (!char.IsDigit(isbn[i]))
            {
                if (i == 9 && isbn[i] == 'X')
                {
                    total += 10 * (10 - i);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                total += (isbn[i] - '0') * (10 - i);
            }
        }

        return total % 11 == 0;
    }


}
