using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gadelshin_Lab1.Data;
using Gadelshin_Lab1.Models;

namespace Gadelshin_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Gadelshin_Lab1Context _context;

        public UsersController(Gadelshin_Lab1Context context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var user = await _context.User
                .Include(u => u.BorrowedBooks)
                .Select(u => new
                {
                    Id = u.Id,
                    Login = u.Login,
                    Role = u.Role,
                    Books = u.BorrowedBooks.Select(b => b.Title).ToList()
                }
                )
                .ToListAsync();
            return Ok(user);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User
                .Include(u => u.BorrowedBooks)
                .Select(u => new
                {
                    Id = u.Id,
                    Login = u.Login,
                    Role = u.Role,
                    Books = u.BorrowedBooks.Select(b => b.Title).ToList()
                }
                )
                .FirstOrDefaultAsync(u => u.Id == id);

            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // GET: /api/users/{id}/borrowed-books
        [HttpGet("{id}/borrowed-books")]
        public async Task<IActionResult> GetBorrowedBooks(int id)
        {
            var user = await _context.User
                .Where(u => u.Id == id)
                .Include(u => u.BorrowedBooks)
                .Select(u => new { Books = u.BorrowedBooks.Select(b => b.Title).ToList() }
                ).FirstOrDefaultAsync();

            await _context.SaveChangesAsync();
            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("Invalid User id. Please check that this {id} is exists");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Users/{id}/add-books
        [HttpPost("{id}/add-books")]
        public async Task<IActionResult> AddUserBooks(int id, [FromBody] List<int> bookIds)
        {
            var user = await _context.User
                .Include(u => u.BorrowedBooks)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var books = await _context.Book
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            if (books == null || books.Count == 0)
            {
                return NotFound("No valid books found with the provided IDs.");
            }

            foreach (var book in books)
            {
                if (!user.BorrowedBooks.Contains(book))
                {
                    user.BorrowedBooks.Add(book);
                    book.UserId = user.Id;
                }
            }
            await _context.SaveChangesAsync();

            return Ok($"Books successfully added to user with ID {id}.");
        }

        // PUT: api/Users/{id}/update-books
        [HttpPut("{id}/update-books")]
        public async Task<IActionResult> UpdateUserBooks(int id, [FromBody] List<int> bookIds)
        {
            var user = await _context.User
                .Include(u => u.BorrowedBooks)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var books = await _context.Book
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            user.BorrowedBooks.Clear();
            user.BorrowedBooks.AddRange(books);
            await _context.SaveChangesAsync();

            return Ok($"Books successfully updated to user with ID {id}.");
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {
            if(user == null || string.IsNullOrEmpty(user.Login) ||
                string.IsNullOrEmpty(user.Role) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid User data. Please provide Login, Rol, and Password.");
            }
            var newUser = new User
            {
                Login = user.Login,
                Role = user.Role,
                Password = user.Password,
                BorrowedBooks = new List<Book>()
            };
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
