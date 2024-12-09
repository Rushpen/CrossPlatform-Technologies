namespace Gadelshin_Lab1.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public int PublishedYear { get; set; }
        public int AuthorId { get; set; }
        virtual public Author Author { get; set; }

        // Methods
        public bool IsAvailable()
        {
            return true;
        }

        public string GetShortDescription()
        {
            return $"{Title} ({PublishedYear}) - {Genre}";
        }
    }
}

