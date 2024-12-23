using System.Text.Json.Serialization;
namespace Gadelshin_Lab1.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Biography { get; set; }

        public List<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }

        // Methods
        public int GetBookCount()
        {
            if (Books == null) return 0;
            return Books.Count;
        }
        public List<string> GetBookTitles()
        {
            List<string> titles = new List<string>();
            foreach (var book in Books)
            {
                if (!string.IsNullOrEmpty(book.Title))
                {
                    titles.Add(book.Title);
                }
            }
            return titles;
        }

        public string GetFullInfo()
        {
            var bookTitles = string.Join(", ", GetBookTitles());
            return $"{Name} ({DateOfBirth.ToString()}) - {Biography}. Books: {bookTitles}";
        }
    }
}
