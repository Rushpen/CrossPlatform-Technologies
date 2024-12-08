namespace Gadelshin_Lab1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } // Admin, Reader и т.д.
        virtual public List<Book> BorrowedBooks { get; set; }

        public User()
        {
            BorrowedBooks = new List<Book>();
        }

        // Methods
        public bool CanBorrowBooks()
        {
            return Role == "Reader";
        }


        public void BorrowBook(Book book)
        {
            if (CanBorrowBooks())
            {
                BorrowedBooks.Add(book);
            }
        }
    }
}
