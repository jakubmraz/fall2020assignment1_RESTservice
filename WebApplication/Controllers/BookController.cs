using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private static List<Book> books = new List<Book>()
        {
            new Book("The Great Gatsby", "F. Scott Fitzgerald", 218, "9780743273565"),
            new Book("The Tin Drum", "Günter Grass", 592, "9780547339108"),
            new Book("Trump: The Art of the Deal", "Donald J. Trump", 400, "9780399594496")
        };

        private Book FindBook(string isbn13)
        {
            Book book = books.FirstOrDefault(data => data.Isbn13 == isbn13);
            return book;
        }

        // GET: api/<BookController>
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            return books;
        }

        // GET api/<BookController>/1234567890123
        [HttpGet("{isbn13}")]
        public IActionResult Get(string isbn13)
        {
            Book book = FindBook(isbn13);
            if (book == null)
            {
                return NotFound(new {message = "Could not find book with the entered ISBN13."});
            }
            return Ok(book);
        }

        // POST api/<BookController>
        [HttpPost]
        public IActionResult Post([FromBody] Book value)
        {
            Book book = FindBook(value.Isbn13);
            if (book != null)
            {
                return Conflict(new {message = "Book already exists."});
            }
            books.Add(value);
            return CreatedAtAction("Get", new {isbn13 = value.Isbn13}, value);
        }

        // PUT api/<BookController>/5
        [HttpPut("{isbn13}")]
        public IActionResult Put(string isbn13, [FromBody] Book value)
        {
            if (isbn13 != value.Isbn13)
            {
                return BadRequest(new {message = "Provided ISBN13 mismatch."});
                
            }

            Book book = FindBook(isbn13);

            if (book != null)
            {
                book.Title = value.Title;
                book.Author = value.Author;
                book.PageNumber = value.PageNumber;
            }
            else
            {
                return NotFound(new {message = "Could not find book with the corresponding ISBN13."});
            }

            return NoContent();
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{isbn13}")]
        public IActionResult Delete(string isbn13)
        {
            Book book = FindBook(isbn13);

            if (book != null)
            {
                books.Remove(book);
            }
            else
            {
                return NotFound(new {message = "Could not find book with the corresponding ISBN13."});
            }

            return Ok(book);
        }
    }
}
