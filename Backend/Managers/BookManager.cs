using Gadelshin_Lab1.Data;
using Gadelshin_Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Gadelshin_Lab1.Managers
{
    public class BookManager
    {
        private readonly Gadelshin_Lab1Context _context;

        public BookManager(Gadelshin_Lab1Context context)
        {
            _context = context;
        }
        public async Task<List<object>> GetBook()
        {
            var books = await _context.Book
                .Select(b => new
                {
                    Id = b.Id,
                    Title = b.Title,
                    Genre = b.Genre,
                    PublishedYear = b.PublishedYear,
                    Authors = b.Authors.Select(b => b.Name).ToList(),
                    isBorrow = b.User != null ? "Yes" : "No"
                }
                )
                .ToListAsync<object>();
            return books;
        }

        public async Task<List<dynamic>> GetFilteredBooks(string? bookType)
        {
            var books = await _context.Book
                .Include(b => b.Authors)
                .Include(b => b.User)
                .ToListAsync();

            if (!string.IsNullOrEmpty(bookType))
            {
                books = books.Where(b => b.GetBookType()
                        .ToString()
                        .Equals(bookType, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var result = books.Select(b => new
            {
                Id = b.Id,
                Title = b.Title,
                Genre = b.Genre,
                PublishedYear = b.PublishedYear,
                Authors = b.Authors.Select(a => a.Name).ToList(),
                isBorrow = b.User != null ? "Yes" : "No"
            }).ToList<dynamic>();

            if (!result.Any())
            {
                throw new Exception("No valid books found with the provided criteria.");
            }
            return result;
        }

        public async Task<List<object>> GetBooksByAuthor(int id)
        {
            return await _context.Book
                .Where(b => b.Authors.Any(a => a.Id == id))
                .Select(b => new
                {
                    Id = b.Id,
                    Title = b.Title,
                    Genre = b.Genre,
                    PublishedYear = b.PublishedYear
                }
                ).ToListAsync<object>();
        }

        public async Task<object> GetBook(int id)
        {
            return await _context.Book
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    Id = b.Id,
                    Title = b.Title,
                    Genre = b.Genre,
                    PublishedYear = b.PublishedYear,
                    Authors = b.Authors.Select(b => b.Name).ToList(),
                }
                ).ToListAsync<object>();
        }

        public async Task<Book> PostBook(Book bookInput)
        {
            if (bookInput == null || string.IsNullOrEmpty(bookInput.Title) ||
                string.IsNullOrEmpty(bookInput.Genre) || bookInput.PublishedYear <= 0)
            {
                throw new Exception("Invalid book data. Please provide Title, Genre, and Published Year.");
            }
            var newBook = new Book
            {
                Title = bookInput.Title,
                Genre = bookInput.Genre,
                PublishedYear = bookInput.PublishedYear,
                Authors = new List<Author>()
            };

            _context.Book.Add(newBook);
            await _context.SaveChangesAsync();

            return newBook;
        }

        public async Task<String> AddAuthorsToBook(int id, [FromBody] List<int> authorIds)
        {
            var book = await _context.Book
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                throw new Exception($"Book with ID {id} not found.");
            }

            var authors = await _context.Author
                .Where(a => authorIds.Contains(a.Id))
                .ToListAsync();

            if (authors == null || authors.Count == 0)
            {
                throw new Exception("No valid authors found with the provided IDs.");
            }

            foreach (var author in authors)
            {
                if (!book.Authors.Contains(author))
                {
                    book.Authors.Add(author);
                }
            }

            await _context.SaveChangesAsync();

            return $"Authors successfully added to the book with ID {id}.";
        }

        public async Task UpdateBook(int id, Book book)
        {
            if (id != book.Id)
                throw new Exception("No valid books found with the provided IDs.");

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuthorsForBook(int id, List<int> authorIds)
        {
            var book = await _context.Book
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                throw new Exception($"Book with ID {id} not found.");

            var authors = await _context.Author
                .Where(a => authorIds.Contains(a.Id))
                .ToListAsync();

            book.Authors.Clear();
            book.Authors.AddRange(authors);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                throw new Exception("No valid books found with the provided IDs.");
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
