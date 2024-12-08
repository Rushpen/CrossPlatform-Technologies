using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gadelshin_Lab1.Models;

namespace Gadelshin_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static List<User> users = new List<User>();

        // GET: /api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return users;
        }

        // GET: /api/users/{id}/borrowed-books
        [HttpGet("{id}/borrowed-books")]
        public IActionResult GetBorrowedBooks(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            return Ok(user.BorrowedBooks);
        }

        // POST: /api/users
        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            users.Add(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        // PUT: /api/users/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            var existingUser = users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null) return NotFound();

            existingUser.Name = user.Name;
            existingUser.Role = user.Role;

            return NoContent();
        }

        // DELETE: /api/users/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            users.Remove(user);
            return NoContent();
        }
    }
}
