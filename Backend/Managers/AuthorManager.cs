using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gadelshin_Lab1.Data;
using Gadelshin_Lab1.Models;
namespace Gadelshin_Lab1.Managers
{
    public class AuthorManager
    {
        private readonly Gadelshin_Lab1Context _context;
        public AuthorManager(Gadelshin_Lab1Context context)
        {
            _context = context;
        }

        public async Task<List<object>> GetAllAuthors()
        {
            return await _context.Author
                .Select(a => new
                {
                    Id = a.Id,
                    Name = a.Name,
                    DateOfBirth = a.DateOfBirth,
                    Biography = a.Biography,
                    Books = a.Books.Select(b => b.Title).ToList()
                })
                .ToListAsync<object>();
        }

        public async Task<List<object>> GetAuthor(int id)
        {
            return await _context.Author
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    Id = a.Id,
                    Name = a.Name,
                    DateOfBirth = a.DateOfBirth,
                    Biography = a.Biography,
                    Books = a.Books.Select(b => b.Title).ToList()
                }
                ).ToListAsync<object>();
        }

        public async Task<String> GetAuthorDetails(int id)
        {
            var author = await _context.Author
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
            return author.GetFullInfo();
        }

        public async Task<Author> PostAuthor(Author author)
        {
            _context.Author.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<String> AddAuthorsToBook(int id, [FromBody] List<int> bookIds)
        {
            var author = await _context.Author
                .Include(b => b.Books)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (author == null)
                throw new Exception($"Author with ID {id} not found.");

            var books = await _context.Book
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            if (books == null || books.Count == 0)
                throw new Exception("No valid books found with the provided IDs.");

            foreach (var book in books)
            {
                if (!author.Books.Contains(book))
                    author.Books.Add(book);
            }

            await _context.SaveChangesAsync();
            return $"Books successfully added to the author with ID {id}.";
        }

        public async Task UpdateAuthor(int id, Author author)
        {
            if (id != author.Id)
                throw new Exception("No valid authors found with the provided IDs.");

            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuthorsToBook(int id, List<int> bookIds)
        {
            var author = await _context.Author
                .Include(b => b.Books)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (author == null)
                throw new Exception($"Author with ID {id} not found.");

            var books = await _context.Book
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            if (books == null || books.Count == 0)
                throw new Exception("No valid books found with the provided IDs.");

            author.Books.Clear();
            author.Books.AddRange(books);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuthor(int id)
        {
            var author = await _context.Author.FindAsync(id);
            if (author == null)
                throw new Exception("No valid authors found with the provided IDs.");

            _context.Author.Remove(author);
            await _context.SaveChangesAsync();
        }

        private bool AuthorExists(int id)
        {
            return _context.Author.Any(e => e.Id == id);
        }
    }
}
