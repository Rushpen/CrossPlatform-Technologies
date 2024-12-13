using Gadelshin_Lab1.Data;
using Gadelshin_Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using NuGet.Packaging.Signing;
using static System.Reflection.Metadata.BlobBuilder;
namespace Gadelshin_Lab1.Managers
{
    public class UserManager
    {
        private readonly Gadelshin_Lab1Context _context;

        public UserManager(Gadelshin_Lab1Context context) 
        {
            _context = context;
        }

        public async Task<List<object>> GetUser()
        {
            return await _context.User
                .Include(u => u.BorrowedBooks)
                .Select(u => new
                {
                    Id = u.Id,
                    Login = u.Login,
                    Role = u.Role,
                    Books = u.BorrowedBooks.Select(b => b.Title).ToList()
                }
                )
                .ToListAsync<object>();
        }

        public async Task<List<object>> GetUser(int id)
        {
            return await _context.User
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    Id = u.Id,
                    Login = u.Login,
                    Role = u.Role,
                    Books = u.BorrowedBooks.Select(b => b.Title).ToList()
                }
                )
                .ToListAsync<object>();
        }

        public async Task<List<object>> GetBorrowedBooks(int id)
        {
            return await _context.User
                .Where(u => u.Id == id)
                .Include(u => u.BorrowedBooks)
                .Select(u => new
                {
                    Books = u.BorrowedBooks.
                    Select(b => b.Title)
                    .ToList() 
                }
                ).ToListAsync<object>();
        }

        public async Task UpdateUser(int id, User user)
        {
            if (id != user.Id)
                throw new Exception("Invalid User id. Please check that this {id} is exists");

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<String> AddUserBooks(int id, [FromBody] List<int> bookIds)
        {
            var user = await _context.User
                .Include(u => u.BorrowedBooks)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }

            var books = await _context.Book
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            if (books == null || books.Count == 0)
            {
                throw new Exception("No valid books found with the provided IDs.");
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

            return $"Books successfully added to user with ID {id}.";
        }

        public async Task UpdateUserBooks(int id, List<int> bookIds)
        {
            var user = await _context.User
                .Include(u => u.BorrowedBooks)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }

            var books = await _context.Book
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            user.BorrowedBooks.Clear();
            user.BorrowedBooks.AddRange(books);
            await _context.SaveChangesAsync();

        }

        public async Task<User> AddUser(User userInput)
        {
            if (userInput == null || string.IsNullOrEmpty(userInput.Login) ||
                string.IsNullOrEmpty(userInput.Role) || string.IsNullOrEmpty(userInput.Password))
            {
                throw new Exception("Invalid User data. Please provide Login, Rol, and Password.");
            }
            var newUser = new User
            {
                Login = userInput.Login,
                Role = userInput.Role,
                Password = userInput.Password,
                BorrowedBooks = new List<Book>()
            };
            _context.User.Add(userInput);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                throw new Exception("No valid users found with the provided IDs.");
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
