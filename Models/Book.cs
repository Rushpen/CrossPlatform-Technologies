using System.Text.Json.Serialization;
namespace Gadelshin_Lab1.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public int PublishedYear { get; set; }
        public int AuthorId { get; set; }
        public int? UserId { get; set; }

        [JsonIgnore]
        public Author? Author { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        // Methods
        public int GetBookAge()
        {
            return DateTime.Now.Year - PublishedYear;
        }
        public bool IsClassic()
        {
            return GetBookAge() > 50;
        }
        public bool IsModern()
        {
            return GetBookAge() <= 10;
        }
        public string GetShortDescription()
        {
            return $"{Title} ({PublishedYear}) - {Genre}, Author: {AuthorId}";
        }
    }
}

