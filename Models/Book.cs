using System.Text.Json.Serialization;
namespace Gadelshin_Lab1.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public int PublishedYear { get; set; }
        public int? UserId { get; set; }

        [JsonIgnore]
        public List<Author>? Authors { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        public enum BookType { Modern, Classic, Other}

        // Methods
        public int GetBookAge()
        {
            return DateTime.Now.Year - PublishedYear;
        }
        public BookType GetBookType()
        {
            int bookAge = GetBookAge();

            if (bookAge <= 10)
                return BookType.Modern; 
            else if (bookAge > 50)
                return BookType.Classic;
            else
                return BookType.Other;
        }

        public string GetShortDescription()
        {
            return $"{Title} ({PublishedYear}) - {Genre}";
        }
    }
}

