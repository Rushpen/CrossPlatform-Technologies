namespace Gadelshin_Lab1.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        virtual public List<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }

        // Methods
        public int GetBookCount()
        {
            return Books.Count;
        }

        public string GetFullInfo()
        {
            var bookTitles = string.Join(", ", Books.Select(b => b.Title));
            return $"{Name} ({DateOfBirth.ToShortDateString()}) - {Biography}. Books: {bookTitles}";
        }
    }
}
