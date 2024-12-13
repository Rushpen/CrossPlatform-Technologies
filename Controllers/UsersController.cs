using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gadelshin_Lab1.Data;
using Gadelshin_Lab1.Models;
using Gadelshin_Lab1.Managers;

namespace Gadelshin_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Gadelshin_Lab1Context _context;
        private readonly UserManager _userManager;

        public UsersController(Gadelshin_Lab1Context context)
        {
            _context = context;
            _userManager = new UserManager(context);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var user = await _userManager.GetUser();
            return Ok(user);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userManager.GetUser(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // GET: /api/users/{id}/borrowed-books
        [HttpGet("{id}/borrowed-books")]
        public async Task<IActionResult> GetBorrowedBooks(int id)
        {
            var user = await _userManager.GetBorrowedBooks(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            try
            {
                await _userManager.UpdateUser(id, user);
                return Ok($"User with ID {id} was updated");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/Users/{id}/add-books
        [HttpPost("{id}/add-books")]
        public async Task<IActionResult> AddUserBooks(int id, [FromBody] List<int> bookIds)
        {
            try
            {
                var message = await _userManager.AddUserBooks(id, bookIds);
                return Ok(message);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Users/{id}/update-books
        [HttpPut("{id}/update-books")]
        public async Task<IActionResult> UpdateUserBooks(int id, [FromBody] List<int> bookIds)
        {
            try
            {
                await _userManager.UpdateUserBooks(id, bookIds);
                return Ok($"Borrowed books updated for user with ID {id}.");
            }
            catch (Exception e) 
            { 
                return BadRequest(e.Message);
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> AddUser([FromBody] User user)
        {
            try
            {
                var message = await _userManager.AddUser(user);
                return Ok(message);
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userManager.DeleteUser(id);
                return Ok($"Book deleted with ID {id}.");
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
