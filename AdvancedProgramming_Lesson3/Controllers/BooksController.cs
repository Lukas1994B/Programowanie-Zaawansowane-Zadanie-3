using AdvancedProgramming_Lesson3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedProgramming_Lesson3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookContext _context;

        public BooksController(BookContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            return await _context.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(long id)
        {
            var book = await _context.TodoItems.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return ItemToDTO(book);
        }

        [HttpPost]
        [Route("UpdateBookItem")]
        public async Task<ActionResult<BookDTO>> UpdateBook(BookDTO bookDTO)
        {
            var book = await _context.TodoItems.FindAsync(bookDTO.Id);
            if (book == null)
            {
                return NotFound();
            }
            book.Title = bookDTO.Title;
            book.Author = bookDTO.Author;
            book.ReleaseCity = bookDTO.ReleaseCity;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BookExists(bookDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdateBook),
                new { id = bookDTO.Id },
                ItemToDTO(book));
        }

        [HttpPost]
        [Route("CreateBookItem")]
        public async Task<ActionResult<BookDTO>> CreateBook(BookDTO bookDTO)
        {
            var bookItem = new Book
            {
                Title = bookDTO.Title,
                Author = bookDTO.Author,
                ReleaseCity = bookDTO.ReleaseCity
            };

            _context.TodoItems.Add(bookItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBook),
                new { id = bookItem.Id },
                ItemToDTO(bookItem));
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(long id)
        {
            var book = await _context.TodoItems.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(book);
            await _context.SaveChangesAsync();

            return book;
        }

        private bool BookExists(long id) => 
            _context.TodoItems.Any(e => e.Id == id);

        private static BookDTO ItemToDTO(Book book) =>
            new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ReleaseCity = book.ReleaseCity
            };
    }
}
